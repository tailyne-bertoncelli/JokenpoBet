using desafio.Context;
using desafio.DataTransferObject.Transactions;
using desafio.Entities;
using desafio.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Sprache;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace desafio.Services
{
    public class TransactionService
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly WalletService _walletService;

        public TransactionService(ApplicationContext context, IHttpContextAccessor httpContext, WalletService walletService)
        {
            _context = context;
            _httpContext = httpContext;
            _walletService = walletService;
        }

        public async Task<ResponseApostaDto> CreateAposta(CreateApostaDto apostaDto)
        {
            var userId = int.Parse(_httpContext.HttpContext.User.FindFirst("Id").Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Wallet wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);

            if (apostaDto.Valor < 1)
            {
                throw new Exception("O valor minimo para apostas é R$1.00!");
            }

            if (wallet.Balance < apostaDto.Valor)
            {
                throw new Exception("Saldo insuficiente para realizar a aposta!");
            }

            if (!Enum.TryParse(apostaDto.Palpite, true, out Jokenpon palpite) || !Enum.IsDefined(typeof(Jokenpon), palpite))
            {
                throw new Exception("Palpite inválido! Tente novamente!");
            }

            Jokenpon result = JokenponSortition();
            Enum.TryParse(apostaDto.Palpite, true, out Jokenpon betting);

            StatusTransaction status = StatusSortition(result, betting);

            var transaction = new Transaction(
               apostaDto.Valor,
               status,
               TypeTransaction.APOSTA,
               user.Id,
               palpite.ToString(),
               result.ToString());

            await _context.Transactions.AddAsync(transaction);

            if (status == StatusTransaction.GANHOU)
            {
                await _walletService.SubtractBalance(user.Id, apostaDto.Valor);
                await WinBet(transaction);
            }
            if(status == StatusTransaction.PERDEU)
            {
                await _walletService.SubtractBalance(user.Id, apostaDto.Valor);
            }

            await VerifyBonus(userId);

            await _context.SaveChangesAsync();

            var transactionResponse = new ResponseApostaDto
            {
                Id = transaction.Id,
                Data = transaction.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                Nome = user.Name,
                Email = user.Email,
                ValorAposta = apostaDto.Valor,
                SaldoEmCarteira = wallet.Balance,
                StatusAposta = transaction.Status,
                Palpite = apostaDto.Palpite.ToString().ToUpper(),
                Resultado = transaction.Result.ToString()
            };

            return transactionResponse;
        }

        public async Task<string> CanceledAposta(long id)
        {
            var userId = int.Parse(_httpContext.HttpContext.User.FindFirst("Id").Value);
            var aposta = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);

            if (aposta == null || aposta.Type == TypeTransaction.PREMIO)
            {
                throw new Exception("Transação de aposta não existe!");
            }

            if (aposta.Status == StatusTransaction.CANCELADA)
            {
                throw new Exception("Transação de aposta já cancelada");
            }

            aposta.Status = StatusTransaction.CANCELADA;

            await _context.SaveChangesAsync();

            return "Transação de aposta cancelada com sucesso!";
        }

        public async Task<PaginationTransaction<TransactionsList>> ListTransactions(RequestTransactions pagination)
        {
            var userId = int.Parse(_httpContext.HttpContext.User.FindFirst("Id").Value);
            var userType = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            var transactions = _context.Transactions.Include(t => t.User).AsQueryable();

                    
            if (userType == "PLAYER")
            {
                transactions = transactions.Where(u => u.UserId == userId);
            } 

            if (pagination.Tipo.HasValue)
            {
                transactions = transactions.Where(t => t.Type == pagination.Tipo);
            }

            var paginationResponse = await transactions.OrderBy(x => x.Id)
                .Skip((pagination.Pagina - 1) * pagination.RegistrosPorPagina)
                .Take(pagination.RegistrosPorPagina)
                .ToListAsync();

            var list = paginationResponse.Select(t => new TransactionsList
            {
                Id = t.Id,
                Data = t.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                Nome = t.User.Name,
                Email = t.User.Email,
                ValorTransacao = t.Value,
                Status = t.Status,
                Tipo = t.Type,
                Palpite = t.Jokenpon,
                Resultado = t.Result
            }).ToList();

            var response = new PaginationTransaction<TransactionsList>(list, transactions.Count(),
                   pagination.Pagina, pagination.RegistrosPorPagina);

            return response;
        }

        private async Task VerifyBonus(long userId)
        {
            var transactions = await _context.Transactions.Include(u => u.User).Where(
                t => t.UserId == userId 
                && t.BonusProcessed == false 
                && t.Type == TypeTransaction.APOSTA 
                && t.Status == StatusTransaction.PERDEU)
                .ToListAsync();

            decimal bonus = 0;

            if (transactions.Count() == 5)
            {
                var valueBonus = await _context.Transactions
                    .Where(t => t.UserId == userId 
                    && t.BonusProcessed == false 
                    && t.Type == TypeTransaction.APOSTA
                    && t.Status == StatusTransaction.PERDEU
                    )
                    .SumAsync(t => t.Value);

                await ApplyBonusToUser(userId, valueBonus);

                foreach (var tra in transactions)
                {
                    tra.BonusProcessed = true;
                }

                await _context.SaveChangesAsync();
            }

        }

        private async Task ApplyBonusToUser(long userId, decimal valueBonus)
        {
            decimal porcent = (valueBonus * 10) / 100;
           
            await _walletService.AddBalance(userId, porcent);

            var addTransaction = new Transaction(porcent, StatusTransaction.CONCLUIDA, TypeTransaction.BONUS, userId);

            await _context.Transactions.AddAsync(addTransaction);
        }

        public Jokenpon JokenponSortition()
        {
            Random random = new Random();
            Array optionsArray = Enum.GetValues(typeof(Jokenpon));

            int draw = random.Next(optionsArray.Length);

            var resultDraw = (Jokenpon)optionsArray.GetValue(draw);

            return resultDraw;
        }

        public StatusTransaction StatusSortition(Jokenpon result, Jokenpon betting)
        {
            StatusTransaction status = StatusTransaction.EM_ANDAMENTO;
            if (betting == result)
            {
                status = StatusTransaction.EMPATE;
            }
            else if (betting == Jokenpon.PAPEL && result == Jokenpon.PEDRA)
            {
                status = StatusTransaction.GANHOU;
            }
            else if (betting == Jokenpon.TESOURA && result == Jokenpon.PAPEL)
            {
                status = StatusTransaction.GANHOU;
            }
            else if (betting == Jokenpon.PEDRA && result == Jokenpon.TESOURA)
            {
                status = StatusTransaction.GANHOU;
            }
            else
            {
                status = StatusTransaction.PERDEU;
            }

            return status;
        }

        private async Task WinBet(Transaction transaction)
        {
            var award = transaction.Value * 2;

            Transaction transactionAward = new Transaction(award, StatusTransaction.CONCLUIDA, 
                TypeTransaction.PREMIO, transaction.UserId);

            await _context.Transactions.AddAsync(transactionAward);

            await _walletService.AddBalance(transaction.UserId, award);

            await _context.SaveChangesAsync();
        }
    }
}

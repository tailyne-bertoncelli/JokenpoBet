using desafio.Context;
using desafio.DataTransferObject.Transactions;
using desafio.Entities.Enum;
using desafio.Entities;
using desafio.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;


namespace DesafioTests
{
    public class TransactionTest
    {
        private readonly TransactionService _transactionService;
        private readonly ApplicationContext _context;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly WalletService _walletService;

        public TransactionTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("Id", "1"),
                new Claim(ClaimTypes.Role, "PLAYER")
            }));

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = user });

            _transactionService = new TransactionService(_context, _httpContextAccessorMock.Object, _walletService);
        }

        [Fact]
        public async Task CanceledApostaTest()
        {
            var transaction = new Transaction(10, StatusTransaction.CONCLUIDA, TypeTransaction.APOSTA, 1);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var result = await _transactionService.CanceledAposta(transaction.Id);

            Assert.Equal("Transação de aposta cancelada com sucesso!", result);
            var canceledTransaction = await _context.Transactions.FindAsync(transaction.Id);
            Assert.Equal(StatusTransaction.CANCELADA, canceledTransaction.Status);
        }

        [Fact]
        public async Task CanceledApostaAlreadyCanceled()
        {
            var transaction = new Transaction(10, StatusTransaction.CANCELADA, TypeTransaction.APOSTA, 1);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<Exception>(() => _transactionService.CanceledAposta(transaction.Id));
            Assert.Equal("Transação de aposta já cancelada", exception.Message);
        }

        [Fact]
        public async Task ListTransactionsTest()
        {
            var userId = 1;

            var transaction1 = new Transaction(10, StatusTransaction.CONCLUIDA, TypeTransaction.APOSTA, userId);
            var transaction2 = new Transaction(20, StatusTransaction.CONCLUIDA, TypeTransaction.APOSTA, userId);
            var transaction3 = new Transaction(15, StatusTransaction.CONCLUIDA, TypeTransaction.APOSTA, userId);
            await _context.Transactions.AddRangeAsync(transaction1, transaction2, transaction3);
            await _context.SaveChangesAsync();

            var total = await _context.Transactions.CountAsync();

            var paginationRequest = new RequestTransactions
            {
                Pagina = 1,
                RegistrosPorPagina = 2
            };

            var result = await _transactionService.ListTransactions(paginationRequest);

            Assert.NotNull(result);
            Assert.Equal(3, total); 
            Assert.Equal(1, result.Pagina);
            Assert.Equal(2, result.RegistroPorPagina); 
        }

        [Fact]
        public async Task JokenponSortitionTests()
        {
            var transactionService = new TransactionService(_context, _httpContextAccessorMock.Object, _walletService);

            var result = transactionService.JokenponSortition();

            Assert.True(Enum.IsDefined(typeof(Jokenpon), result));
        }

        [Fact]
        public async Task StatusSortitionPapelTest()
        {
            var betting = Jokenpon.PAPEL; 
            var result = Jokenpon.PEDRA; 

            var request = _transactionService.StatusSortition(result, betting); 

            Assert.Equal(StatusTransaction.GANHOU, request);
        }

        [Fact]
        public async Task StatusSortitionPedraTest()
        {
            var betting = Jokenpon.PEDRA;
            var result = Jokenpon.TESOURA;

            var request = _transactionService.StatusSortition(result, betting);

            Assert.Equal(StatusTransaction.GANHOU, request);
        }

        [Fact]
        public async Task StatusSortitionTesouraTest()
        {
            var betting = Jokenpon.TESOURA;
            var result = Jokenpon.PAPEL;

            var request = _transactionService.StatusSortition(result, betting);

            Assert.Equal(StatusTransaction.GANHOU, request);
        }

        [Fact]
        public async Task StatusSortitionEmpateTest()
        {
            var betting = Jokenpon.TESOURA;
            var result = Jokenpon.TESOURA;

            var request = _transactionService.StatusSortition(result, betting);

            Assert.Equal(StatusTransaction.EMPATE, request);
        }
    }
}

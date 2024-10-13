using desafio.Context;
using desafio.DataTransferObject.User;
using desafio.Entities;
using desafio.Entities.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace desafio.Services
{
    public class UserService
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ApplicationContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UsersDto> Create(CreateUserDto userDto)
        {
            User user = new User(
                userDto.Nome,
                userDto.Email,
                userDto.Senha,
                userDto.TipoUsuario
                );

            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            decimal balance = 0;

            if (user.UserType == UserType.PLAYER) {
                Wallet wallet = new Wallet(user.Id, userDto.SaldoInicial, userDto.Moeda);
                balance = wallet.Balance;
                await _context.Wallet.AddAsync(wallet);
                await _context.SaveChangesAsync();
            }

            var userResponse = new UsersDto
            {
                Id = user.Id,
                Nome = user.Name,
                Email = user.Email,
                TipoUsuario = user.UserType,
                SaldoEmCarteira = balance
            };

            return userResponse;
            
        }

        public async Task<string> UpdatePassword(UpdatePasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            user.Password = _passwordHasher.HashPassword(user, dto.NovaSenha);
            await _context.SaveChangesAsync();
            return "Senha atualizada com sucesso!";
        }

        public async Task<List<UsersDto>> GetAllUsers()
        {
            var users = await _context.Users.Include(u => u.Wallet).ToListAsync();

            var listUsers = users.Select(users => new UsersDto
            {
                Id = users.Id,
                Nome = users.Name,
                Email = users.Email,
                SaldoEmCarteira = users.Wallet != null ? users.Wallet.Balance : 0,
                TipoUsuario = users.UserType
            }).ToList();

            return listUsers;
        }
        public async Task<bool> VerifyUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
    }
}

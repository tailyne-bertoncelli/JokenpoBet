using desafio.Context;
using desafio.DataTransferObject.User;
using desafio.Entities;
using desafio.Entities.Enum;
using desafio.Services;
using Microsoft.EntityFrameworkCore;

namespace DesafioTests
{
    public class UserTests
    {
        private readonly UserService _userService;
        private readonly ApplicationContext _context;

        public UserTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                      .Options;
            _context = new ApplicationContext(options);
            _userService = new UserService(_context);
        }

        [Fact]
        public async Task CreateUserTestAsync()
        {
            var createUserDto = new CreateUserDto
            {
                Nome = "Tailyne Bertoncelli",
                Email = "tailyne.berton@gmail.com",
                Senha = "12345678",
                TipoUsuario = UserType.PLAYER,
                SaldoInicial = 100,
                Moeda = Currency.REAL_BRL
            };

            var result = await _userService.Create(createUserDto);

            Assert.NotNull(result);
            Assert.Equal("Tailyne Bertoncelli", result.Nome);
            Assert.Equal("tailyne.berton@gmail.com", result.Email);
            Assert.Equal(UserType.PLAYER, result.TipoUsuario);
            Assert.Equal(100, result.SaldoEmCarteira);

            var wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == result.Id);
            Assert.NotNull(wallet);
            Assert.Equal(100, wallet.Balance);
        }

        [Fact]
        public async Task UpdatePassword() {
            var user = new User("Tailyne Bertoncelli", "tailyne.berton@gmail.com", "12345678", UserType.ADMIN);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var updatePasswordDto = new UpdatePasswordDto
            {
                Email = "tailyne.berton@gmail.com",
                NovaSenha = "87654321"
            };

            var result = await _userService.UpdatePassword(updatePasswordDto);

            Assert.Equal("Senha atualizada com sucesso!", result);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "tailyne.berton@gmail.com");
            Assert.NotNull(updatedUser);
            Assert.NotEqual("87654321", updatedUser.Password);
        }

        [Fact]
        public async Task GetAllUsersTest()
        {
            var tailyne = new User("Tailyne Bertoncelli", "tailyne@gmail.com", "12345678", UserType.PLAYER)
            {
                Wallet = new Wallet(1, 100, Currency.REAL_BRL)
            };
            var maria = new User("Maria", "maria@gmail.com", "87654321", UserType.ADMIN);
            _context.Users.AddRange(tailyne, maria);
            await _context.SaveChangesAsync();

            var result = await _userService.GetAllUsers();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.Email == "tailyne@gmail.com" && u.SaldoEmCarteira == 100);
            Assert.Contains(result, u => u.Email == "maria@gmail.com" && u.SaldoEmCarteira == 0);
        }

        [Fact]
        public async Task VeriVerifyUserByEmailNotExistTest()
        {
            var result = await _userService.VerifyUserByEmail("tailyne@gmail.com");

            Assert.False(result);
        }
    }
}
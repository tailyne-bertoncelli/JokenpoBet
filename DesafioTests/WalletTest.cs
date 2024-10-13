using desafio.Context;
using desafio.Entities;
using desafio.Services;
using Microsoft.EntityFrameworkCore;

namespace DesafioTests
{
    public class WalletTest
    {
        private readonly ApplicationContext _context;
        private readonly WalletService _walletService;
        public WalletTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationContext(options);
            _walletService = new WalletService(_context);
        }

        [Fact]
        public async Task AddBalanceTest()
        {
            var userId = 1;
            var initialBalance = 100;
            var amountToAdd = 50;

            var wallet = new Wallet { UserId = userId, Balance = initialBalance };
            await _context.Wallet.AddAsync(wallet);
            await _context.SaveChangesAsync();
            
            await _walletService.AddBalance(userId, amountToAdd);

            var updatedWallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
            Assert.NotNull(updatedWallet);
            Assert.Equal(150, updatedWallet.Balance);
        }

        [Fact]
        public async Task SubtractBalanceTest()
        {
            var userId = 1;
            var initialBalance = 100;
            var amountToSubtract = 50;

            var wallet = new Wallet { UserId = userId, Balance = initialBalance };
            await _context.Wallet.AddAsync(wallet);
            await _context.SaveChangesAsync();

            await _walletService.SubtractBalance(userId, amountToSubtract);

            var updatedWallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
            Assert.NotNull(updatedWallet);
            Assert.Equal(50, updatedWallet.Balance);
        }
    }
}

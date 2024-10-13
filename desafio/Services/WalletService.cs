using desafio.Context;
using desafio.Entities;
using Microsoft.EntityFrameworkCore;

namespace desafio.Services
{
    public class WalletService
    {
        private readonly ApplicationContext _context;
        public WalletService(ApplicationContext context) {
            _context = context;
        }

        public async Task AddBalance(long userId, decimal value)
        {
            Wallet wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
            wallet.Balance += value;

            await _context.SaveChangesAsync();
        }

        public async Task SubtractBalance(long userId, decimal value)
        {
            Wallet wallet = await _context.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
            wallet.Balance -= value;

            await _context.SaveChangesAsync();
        }
    }
}

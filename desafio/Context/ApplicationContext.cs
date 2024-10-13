using desafio.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace desafio.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Wallet> Wallet {  get; set; }
    }
}

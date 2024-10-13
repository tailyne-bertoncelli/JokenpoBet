using desafio.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio.Entities
{
    [Table("tb_wallet")]
    public class Wallet : AbstractEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("balance")]
        public decimal Balance { get; set; }

        [Column("currency", TypeName = "varchar(100)")]
        public Currency Currency { get; set; }

        public Wallet() { }

        public Wallet(long userId, decimal balance, Currency currency) {
            UserId = userId;
            Balance = balance;
            Currency = currency;
        }
    }
}

using desafio.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace desafio.Entities
{
    [Table("tb_transaction")]
    public class Transaction : AbstractEntity
    {
        [Column("value")]
        public decimal Value { get; set; }

        [Column("status", TypeName = "varchar(100)")]
        public StatusTransaction Status { get; set; }

        [Column("type", TypeName = "varchar(100)")]
        public TypeTransaction Type { get; set; }

        [Column("bonus_processed")]
        public bool BonusProcessed { get; set; } = false;

        [Column("betting", TypeName = "varchar(100)")]
        public string? Jokenpon { get; set; }

        [Column("result", TypeName = "varchar(100)")]
        public string? Result { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        public Transaction() { }

        public Transaction(decimal value, StatusTransaction status, TypeTransaction type, long user, string jokenpon, string result)
        {
            Value = value;
            Status = status;
            Type = type;
            UserId = user;
            Jokenpon = jokenpon;
            Result = result;
        }

        public Transaction(decimal value, StatusTransaction status, TypeTransaction type, long user)
        {
            Value = value;
            Status = status;
            Type = type;
            UserId = user;
        }
    }
}

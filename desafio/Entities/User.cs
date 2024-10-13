using desafio.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace desafio.Entities
{
    [Table("tb_user")]
    public class User : AbstractEntity
    {
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }

        [Column("email", TypeName = "varchar(100)")]
        [EmailAddress, Required]
        public string Email { get; set; }

        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; }

        [Column("user_type", TypeName = "varchar(100)")]
        public UserType UserType { get; set; }

        public Wallet Wallet { get; set; }

        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        public User() { }

        public User(string name, string email, string password, UserType userType)
        {
            Name = name;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }
}

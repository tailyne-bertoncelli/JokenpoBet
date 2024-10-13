using desafio.Entities.Enum;

namespace desafio.DataTransferObject.User
{
    public class UsersDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public UserType TipoUsuario { get; set; }
        public decimal SaldoEmCarteira { get; set; }
        public Currency Moeda { get; set; }
    }
}

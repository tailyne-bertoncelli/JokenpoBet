using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Login
{
    public class ResponseLoginDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public UserType TipoUsuario { get; set; }
        public decimal? SaldoEmCarteira { get; set; }
        public string Token { get; set; }
    }
}

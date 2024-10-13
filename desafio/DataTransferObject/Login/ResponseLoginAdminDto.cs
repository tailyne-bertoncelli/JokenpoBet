using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Login
{
    public class ResponseLoginAdminDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public UserType TipoUsuario { get; set; }
        public string Token { get; set; }
    }
}

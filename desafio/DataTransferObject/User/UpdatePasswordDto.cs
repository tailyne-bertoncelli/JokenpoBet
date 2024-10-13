namespace desafio.DataTransferObject.User
{
    public class UpdatePasswordDto
    {
        public string Email { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }
}

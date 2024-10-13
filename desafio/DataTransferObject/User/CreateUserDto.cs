using desafio.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace desafio.DataTransferObject.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Tipo de usuário é obrigatório")]
        public UserType TipoUsuario { get; set; }

        [Required(ErrorMessage = "Saldo Inicial é obrigatório")]
        public decimal SaldoInicial { get; set; }

        [Required(ErrorMessage = "Moeda é obrigatório")]
        public Currency Moeda { get; set; }
    }
}

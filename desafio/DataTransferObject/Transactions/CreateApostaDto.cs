using desafio.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace desafio.DataTransferObject.Transactions
{
    public class CreateApostaDto
    {
        [Required(ErrorMessage = "Valor é obrigatório")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Palpite é obrigatório")]
        public string Palpite { get; set; }
    }
}

using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Transactions
{
    public class ResponseApostaDto
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public decimal ValorAposta { get; set; }
        public decimal SaldoEmCarteira { get; set; }
        public StatusTransaction StatusAposta { get; set; }
        public string Palpite { get; set; }
        public string Resultado { get; set; }
    }
}

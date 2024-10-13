using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Transactions
{
    public class TransactionsList
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public decimal ValorTransacao { get; set; }
        public TypeTransaction Tipo { get; set; }
        public StatusTransaction Status { get; set; }
        public string Palpite { get; set; }
        public string Resultado { get; set; }
    }
}

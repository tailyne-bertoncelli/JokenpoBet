using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Transactions
{
    public class ApostaList
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public decimal Valor {  get; set; }
        public StatusTransaction StatusTransacao { get; set; }
        public TypeTransaction Tipo { get; set; }
        public string Palpite { get; set; }
        public string Resultado { get; set; }
    }
}

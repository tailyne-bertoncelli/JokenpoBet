using desafio.Entities.Enum;

namespace desafio.DataTransferObject.Transactions
{
    public class RequestTransactions
    {
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public TypeTransaction? Tipo { get; set; }
    }
}

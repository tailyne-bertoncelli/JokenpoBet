namespace desafio.DataTransferObject.Transactions
{
    public class PaginationTransaction<T>
    {
        public List<T> Transactions { get; set; }
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int RegistroPorPagina { get; set; }

        public PaginationTransaction(List<T> items, int total, int pagina, int registros)
        {
            Transactions = items;
            Total = total;
            Pagina = pagina;
            RegistroPorPagina = registros;
        }
    }
}

namespace PortfolioManagement.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoFinanceiroId { get; set; }
        public DateTime DataTransacao { get; set; }
        public decimal Valor { get; set; }
        public string TipoTransacao { get; set; } // Compra ou Venda
    }
}

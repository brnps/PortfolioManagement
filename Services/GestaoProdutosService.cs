using PortfolioManagement.Models;

namespace PortfolioManagement.Services
{
    public class GestaoProdutosService
    {
        private readonly List<ProdutoFinanceiro> produtos;

        public GestaoProdutosService()
        {
            produtos = new List<ProdutoFinanceiro>();
        }

        public void AdicionarProduto(ProdutoFinanceiro produto)
        {
            produtos.Add(produto);
        }

        public void AtualizarProduto(ProdutoFinanceiro produto)
        {
            var produtoExistente = produtos.FirstOrDefault(p => p.Id == produto.Id);
            if (produtoExistente != null)
            {
                produtoExistente.Nome = produto.Nome;
                produtoExistente.DataVencimento = produto.DataVencimento;
                produtoExistente.Valor = produto.Valor;
            }
        }

        public void RemoverProduto(int id)
        {
            var produto = produtos.FirstOrDefault(p => p.Id == id);
            if (produto != null)
            {
                produtos.Remove(produto);
            }
        }

        public List<ProdutoFinanceiro> ListarProdutos()
        {
            return produtos;
        }
    }
}

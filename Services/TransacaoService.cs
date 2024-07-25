using PortfolioManagement.Models;

namespace PortfolioManagement.Services
{
    public class TransacaoService
    {
        private readonly List<Transacao> transacoes;
        private readonly List<ProdutoFinanceiro> produtos;
        private readonly List<Cliente> clientes;

        public TransacaoService()
        {
            transacoes = new List<Transacao>();
            produtos = new List<ProdutoFinanceiro>(); // Simulação de produtos existentes
            clientes = new List<Cliente>(); // Simulação de clientes existentes
        }

        public void RealizarCompra(Transacao transacao)
        {
            // Verificar se o produto e o cliente existem
            var produto = produtos.FirstOrDefault(p => p.Id == transacao.ProdutoFinanceiroId);
            var cliente = clientes.FirstOrDefault(c => c.Id == transacao.ClienteId);

            if (produto != null && cliente != null)
            {
                transacao.TipoTransacao = "Compra";
                transacao.DataTransacao = DateTime.Now;
                transacoes.Add(transacao);
            }
            else
            {
                throw new Exception("Produto ou Cliente não encontrado.");
            }
        }

        public void RealizarVenda(Transacao transacao)
        {            
            var produto = produtos.FirstOrDefault(p => p.Id == transacao.ProdutoFinanceiroId);
            var cliente = clientes.FirstOrDefault(c => c.Id == transacao.ClienteId);

            if (produto != null && cliente != null)
            {
                transacao.TipoTransacao = "Venda";
                transacao.DataTransacao = DateTime.Now;
                transacoes.Add(transacao);
            }
            else
            {
                throw new Exception("Produto ou Cliente não encontrado.");
            }
        }

        public List<Transacao> ObterExtrato(int clienteId)
        {
            return transacoes.Where(t => t.ClienteId == clienteId).ToList();
        }

        public void AdicionarProduto(ProdutoFinanceiro produto)
        {
            produtos.Add(produto);
        }

        public void AdicionarCliente(Cliente cliente)
        {
            clientes.Add(cliente);
        }
    }
}

using Microsoft.Extensions.Logging;
using PortfolioManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioManagement.Services
{
    public class TransacaoService
    {
        private readonly List<Transacao> transacoes;
        private readonly List<ProdutoFinanceiro> produtos;
        private readonly List<Cliente> clientes;
        private readonly ILogger<TransacaoService> _logger;

        public TransacaoService(ILogger<TransacaoService> logger)
        {
            transacoes = new List<Transacao>();
            produtos = new List<ProdutoFinanceiro>(); // Simulação de produtos existentes
            clientes = new List<Cliente>(); // Simulação de clientes existentes
            _logger = logger;
        }

        public void RealizarCompra(Transacao transacao)
        {
            try
            {
                var produto = produtos.FirstOrDefault(p => p.Id == transacao.ProdutoFinanceiroId);
                var cliente = clientes.FirstOrDefault(c => c.Id == transacao.ClienteId);

                if (produto != null && cliente != null)
                {
                    transacao.TipoTransacao = "Compra";
                    transacao.DataTransacao = DateTime.Now;
                    transacoes.Add(transacao);

                    _logger.LogInformation("Compra realizada com sucesso: {@Transacao}", transacao);
                }
                else
                {
                    throw new Exception("Produto ou Cliente não encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar compra.");
                throw; 
            }
        }

        public void RealizarVenda(Transacao transacao)
        {
            try
            {
                var produto = produtos.FirstOrDefault(p => p.Id == transacao.ProdutoFinanceiroId);
                var cliente = clientes.FirstOrDefault(c => c.Id == transacao.ClienteId);

                if (produto != null && cliente != null)
                {
                    transacao.TipoTransacao = "Venda";
                    transacao.DataTransacao = DateTime.Now;
                    transacoes.Add(transacao);

                    _logger.LogInformation("Venda realizada com sucesso: {@Transacao}", transacao);
                }
                else
                {
                    throw new Exception("Produto ou Cliente não encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar venda.");
                throw; 
            }
        }

        public List<Transacao> ObterExtrato(int clienteId)
        {
            try
            {
                var extrato = transacoes.Where(t => t.ClienteId == clienteId).ToList();

                _logger.LogInformation("Extrato obtido com sucesso para o cliente ID {ClienteId}", clienteId);
                return extrato;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter extrato para o cliente ID {ClienteId}", clienteId);
                throw; // Re-lançar a exceção para que o controlador possa lidar com ela
            }
        }

        public void AdicionarProduto(ProdutoFinanceiro produto)
        {
            try
            {
                produtos.Add(produto);
                _logger.LogInformation("Produto adicionado com sucesso: {@Produto}", produto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto.");
                throw; 
            }
        }

        public void AdicionarCliente(Cliente cliente)
        {
            try
            {
                clientes.Add(cliente);
                _logger.LogInformation("Cliente adicionado com sucesso: {@Cliente}", cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar cliente.");
                throw; 
            }
        }
    }
}

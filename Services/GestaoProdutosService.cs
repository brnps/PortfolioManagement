using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PortfolioManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioManagement.Services
{
    public class GestaoProdutosService
    {
        private readonly List<ProdutoFinanceiro> produtos;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<GestaoProdutosService> _logger;

        public GestaoProdutosService(IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<GestaoProdutosService> logger)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _logger = logger;
            produtos = new List<ProdutoFinanceiro>();
        }

        public void AdicionarProduto(ProdutoFinanceiro produto)
        {
            try
            {
                produtos.Add(produto);
                AtualizarCache().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto.");
                throw;
            }
        }

        public void AtualizarProduto(ProdutoFinanceiro produto)
        {
            try
            {
                var produtoExistente = produtos.FirstOrDefault(p => p.Id == produto.Id);
                if (produtoExistente != null)
                {
                    produtoExistente.Nome = produto.Nome;
                    produtoExistente.DataVencimento = produto.DataVencimento;
                    produtoExistente.Valor = produto.Valor;
                    AtualizarCache().Wait();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto.");
                throw;
            }
        }

        public void RemoverProduto(int id)
        {
            try
            {
                var produto = produtos.FirstOrDefault(p => p.Id == id);
                if (produto != null)
                {
                    produtos.Remove(produto);
                    AtualizarCache().Wait();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover produto.");
                throw;
            }
        }

        public async Task<List<ProdutoFinanceiro>> ListarProdutosAsync()
        {
            try
            {
                var cachedProdutos = await _distributedCache.GetStringAsync("produtos");

                if (cachedProdutos != null)
                {
                    return JsonConvert.DeserializeObject<List<ProdutoFinanceiro>>(cachedProdutos);
                }

                await AtualizarCache();
                return produtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar produtos.");
                throw;
            }
        }

        private async Task AtualizarCache()
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            try
            {
                await _distributedCache.SetStringAsync("produtos", JsonConvert.SerializeObject(produtos), cacheOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cache.");
                throw;
            }
        }
    }
}

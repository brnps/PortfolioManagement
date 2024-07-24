using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly GestaoProdutosService _gestaoProdutosService;

        public ProdutosController(GestaoProdutosService gestaoProdutosService)
        {
            _gestaoProdutosService = gestaoProdutosService;
        }

        [HttpPost]
        public IActionResult AdicionarProduto([FromBody] ProdutoFinanceiro produto)
        {
            _gestaoProdutosService.AdicionarProduto(produto);
            return Ok();
        }

        [HttpPut]
        public IActionResult AtualizarProduto([FromBody] ProdutoFinanceiro produto)
        {
            _gestaoProdutosService.AtualizarProduto(produto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverProduto(int id)
        {
            _gestaoProdutosService.RemoverProduto(id);
            return Ok();
        }

        [HttpGet]
        public IActionResult ListarProdutos()
        {
            var produtos = _gestaoProdutosService.ListarProdutos();
            return Ok(produtos);
        }
    }
}

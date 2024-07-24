using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly TransacaoService _transacaoService;

        public TransacoesController(TransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        [HttpPost("comprar")]
        public IActionResult RealizarCompra([FromBody] Transacao transacao)
        {
            try
            {
                _transacaoService.RealizarCompra(transacao);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("vender")]
        public IActionResult RealizarVenda([FromBody] Transacao transacao)
        {
            try
            {
                _transacaoService.RealizarVenda(transacao);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{clienteId}")]
        public IActionResult ObterExtrato(int clienteId)
        {
            var extrato = _transacaoService.ObterExtrato(clienteId);
            return Ok(extrato);
        }
    }
}

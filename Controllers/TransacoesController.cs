using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PortfolioManagement.Models;
using PortfolioManagement.Services;
using System;

namespace PortfolioManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly TransacaoService _transacaoService;
        private readonly ILogger<TransacoesController> _logger;

        public TransacoesController(TransacaoService transacaoService, ILogger<TransacoesController> logger)
        {
            _transacaoService = transacaoService;
            _logger = logger;
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
                _logger.LogError(ex, "Erro ao realizar compra.");
                return StatusCode(500, "Ocorreu um erro ao realizar a compra.");
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
                _logger.LogError(ex, "Erro ao realizar venda.");
                return StatusCode(500, "Ocorreu um erro ao realizar a venda.");
            }
        }

        [HttpGet("{clienteId}")]
        public IActionResult ObterExtrato(int clienteId)
        {
            try
            {
                var extrato = _transacaoService.ObterExtrato(clienteId);
                return Ok(extrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter extrato.");
                return StatusCode(500, "Ocorreu um erro ao obter o extrato.");
            }
        }
    }
}

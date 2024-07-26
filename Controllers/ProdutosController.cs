using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly GestaoProdutosService _produtoService;

    public ProdutoController(GestaoProdutosService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var produtos = await _produtoService.ListarProdutosAsync();
            return Ok(produtos);
        }
        catch (Exception ex)
        {
            // Log de exceção
            return StatusCode(500, "Ocorreu um erro ao processar a requisição.");
        }
    }

    [HttpPost]
    public IActionResult Post([FromBody] ProdutoFinanceiro produto)
    {
        try
        {
            _produtoService.AdicionarProduto(produto);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log de exceção
            return StatusCode(500, "Ocorreu um erro ao processar a requisição.");
        }
    }

    [HttpPut]
    public IActionResult Put([FromBody] ProdutoFinanceiro produto)
    {
        try
        {
            _produtoService.AtualizarProduto(produto);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log de exceção
            return StatusCode(500, "Ocorreu um erro ao processar a requisição.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _produtoService.RemoverProduto(id);
            return Ok();
        }
        catch (Exception ex)
        {
            // Log de exceção
            return StatusCode(500, "Ocorreu um erro ao processar a requisição.");
        }
    }
}

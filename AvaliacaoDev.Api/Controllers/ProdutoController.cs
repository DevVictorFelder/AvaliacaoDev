using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoDev.Api.Controllers
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _service;

        public ProdutoController(IProdutoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(ProdutoDto dto)
        {
            var id = await _service.Criar(dto);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _service.Listar());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            return Ok(await _service.Obter(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, ProdutoDto dto)
        {
            await _service.Atualizar(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            await _service.Deletar(id);
            return Ok();
        }
    }
}
using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvaliacaoDev.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de pedidos via API REST.
    /// Expõe endpoints para operações CRUD e fluxo de pedidos.
    /// </summary>
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;

        /// <summary>
        /// Construtor do controller com injeção de dependência
        /// </summary>
        /// <param name="service">Serviço de gerenciamento de pedidos</param>
        public PedidoController(IPedidoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo pedido no sistema
        /// </summary>
        /// <param name="clienteId">ID opcional do cliente. Se não fornecido, usará cliente padrão</param>
        /// <returns>ID do pedido criado</returns>
        /// <response code="200">Pedido criado com sucesso</response>
        /// <response code="400">Cliente informado não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        public async Task<IActionResult> Criar([FromQuery] Guid? clienteId = null)
        {
            try
            {
                var id = await _service.CriarPedido(clienteId);
                return Ok(new { PedidoId = id, Message = "Pedido criado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Adiciona um produto a um pedido existente
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <param name="dto">Dados do produto a ser adicionado</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Produto adicionado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("{id}/produtos")]
        public async Task<IActionResult> Adicionar(Guid id, [FromBody] ProdutoDto dto)
        {
            try
            {
                await _service.AdicionarProduto(id, dto);
                return Ok("Produto adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao adicionar produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Remove um produto de um pedido
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <param name="produtoId">ID do produto a ser removido</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Produto removido com sucesso</response>
        /// <response code="400">Pedido fechado ou produto não encontrado</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpDelete("{id}/produtos/{produtoId}")]
        public async Task<IActionResult> Remover(Guid id, Guid produtoId)
        {
            try
            {
                await _service.RemoverProduto(id, produtoId);
                return Ok("Produto removido com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao remover produto: {ex.Message}");
            }
        }

        /// <summary>
        /// Fecha um pedido, impedindo novas alterações
        /// </summary>
        /// <param name="id">ID do pedido a ser fechado</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Pedido fechado com sucesso</response>
        /// <response code="400">Pedido não pode ser fechado</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("{id}/fechar")]
        public async Task<IActionResult> Fechar(Guid id)
        {
            try
            {
                await _service.FecharPedido(id);
                return Ok("Pedido fechado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao fechar pedido: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista pedidos com suporte a paginação e filtro
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
        /// <param name="status">Filtro opcional por status</param>
        /// <returns>Lista de pedidos paginada</returns>
        /// <response code="200">Lista de pedidos retornada com sucesso</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="500">Erro interno</response>
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string status = null)
        {
            try
            {
                if (page < 1) return BadRequest("Page deve ser maior que 0");
                if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize deve estar entre 1 e 100");
                
                var result = await _service.Listar(page, pageSize, status);
                return Ok(new { Data = result, Page = page, PageSize = pageSize, Total = result.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém os dados completos de um pedido específico
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Dados completos do pedido</returns>
        /// <response code="200">Pedido encontrado</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Obter(Guid id)
        {
            try
            {
                var result = await _service.Obter(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
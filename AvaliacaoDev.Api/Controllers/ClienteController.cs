using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AvaliacaoDev.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de clientes via API REST.
    /// Expõe endpoints para operações CRUD de clientes.
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;

        /// <summary>
        /// Construtor do controller com injeção de dependência
        /// </summary>
        /// <param name="service">Serviço de gerenciamento de clientes</param>
        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Cria um novo cliente no sistema
        /// </summary>
        /// <param name="dto">Dados do cliente a ser criado</param>
        /// <returns>ID do cliente criado</returns>
        /// <response code="200">Cliente criado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ClienteDto dto)
        {
            try
            {
                var id = await _service.Criar(dto);
                return Ok(new { ClienteId = id, Message = "Cliente criado com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um cliente específico pelo ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Dados completos do cliente</returns>
        /// <response code="200">Cliente encontrado</response>
        /// <response code="404">Cliente não encontrado</response>
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
                if (ex.Message.Contains("não encontrado"))
                    return NotFound(new { Message = ex.Message });
                
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Lista todos os clientes cadastrados
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
        /// <returns>Lista de clientes paginada</returns>
        /// <response code="200">Lista de clientes retornada com sucesso</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="500">Erro interno</response>
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) return BadRequest("Page deve ser maior que 0");
                if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize deve estar entre 1 e 100");
                
                var result = await _service.Listar(page, pageSize);
                return Ok(new { Data = result, Page = page, PageSize = pageSize, Total = result.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <param name="dto">Dados atualizados do cliente</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Cliente atualizado com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClienteDto dto)
        {
            try
            {
                await _service.Atualizar(id, dto);
                return Ok("Cliente atualizado com sucesso");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("não encontrado"))
                    return NotFound(new { Message = ex.Message });
                
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Remove um cliente do sistema
        /// </summary>
        /// <param name="id">ID do cliente a ser removido</param>
        /// <returns>Resultado da operação</returns>
        /// <response code="200">Cliente removido com sucesso</response>
        /// <response code="404">Cliente não encontrado</response>
        /// <response code="500">Erro interno</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _service.Deletar(id);
                return Ok("Cliente removido com sucesso");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("não encontrado"))
                    return NotFound(new { Message = ex.Message });
                
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
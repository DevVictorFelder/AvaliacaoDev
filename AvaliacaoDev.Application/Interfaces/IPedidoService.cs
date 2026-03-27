using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaliacaoDev.Application.DTOs;

namespace AvaliacaoDev.Application.Interfaces
{
    /// <summary>
    /// Interface de serviço para gerenciamento de pedidos.
    /// Define o contrato para casos de uso relacionados a pedidos.
    /// </summary>
    public interface IPedidoService
    {
        /// <summary>
        /// Cria um novo pedido no sistema
        /// </summary>
        /// <param name="clienteId">ID opcional do cliente. Se não fornecido, usará cliente padrão</param>
        /// <returns>ID do pedido criado</returns>
        Task<Guid> CriarPedido(Guid? clienteId = null);
        
        /// <summary>
        /// Adiciona um produto a um pedido existente
        /// </summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <param name="dto">Dados do produto a ser adicionado</param>
        Task AdicionarProduto(Guid pedidoId, ProdutoDto dto);
        
        /// <summary>
        /// Remove um produto de um pedido
        /// </summary>
        /// <param name="pedidoId">ID do pedido</param>
        /// <param name="produtoId">ID do produto a ser removido</param>
        Task RemoverProduto(Guid pedidoId, Guid produtoId);
        
        /// <summary>
        /// Fecha um pedido, impedindo novas alterações
        /// </summary>
        /// <param name="pedidoId">ID do pedido a ser fechado</param>
        Task FecharPedido(Guid pedidoId);
        
        /// <summary>
        /// Lista pedidos com paginação e filtro opcional
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="status">Filtro opcional por status</param>
        /// <returns>Lista de pedidos</returns>
        Task<List<PedidoDto>> Listar(int page, int pageSize, string status);
        
        /// <summary>
        /// Obtém um pedido específico pelo ID
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Dados completos do pedido</returns>
        Task<PedidoDto> Obter(Guid id);
    }
}
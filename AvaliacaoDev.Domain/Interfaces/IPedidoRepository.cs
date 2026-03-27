using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaliacaoDev.Domain.Entities;

namespace AvaliacaoDev.Domain.Interfaces
{
    /// <summary>
    /// Interface de repositório para acesso a dados de pedidos.
    /// Define o contrato para operações CRUD e consultas de pedidos.
    /// </summary>
    public interface IPedidoRepository
    {
        /// <summary>
        /// Adiciona um novo pedido ao repositório
        /// </summary>
        /// <param name="pedido">Pedido a ser adicionado</param>
        Task Add(Pedido pedido);
        
        /// <summary>
        /// Atualiza um pedido existente no repositório
        /// </summary>
        /// <param name="pedido">Pedido a ser atualizado</param>
        Task Update(Pedido pedido);
        
        /// <summary>
        /// Busca um pedido pelo seu ID
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Pedido encontrado ou null</returns>
        Task<Pedido> GetById(Guid id);
        
        /// <summary>
        /// Lista pedidos com paginação e filtro opcional por status
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="status">Filtro opcional por status</param>
        /// <returns>Lista de pedidos</returns>
        Task<List<Pedido>> GetAll(int page, int pageSize, string status);
    }
}
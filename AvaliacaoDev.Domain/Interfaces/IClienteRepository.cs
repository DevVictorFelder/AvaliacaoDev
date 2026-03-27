using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaliacaoDev.Domain.Entities;

namespace AvaliacaoDev.Domain.Interfaces
{
    /// <summary>
    /// Interface de repositório para acesso a dados de clientes.
    /// Define o contrato para operações CRUD e consultas de clientes.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Adiciona um novo cliente ao repositório
        /// </summary>
        /// <param name="cliente">Cliente a ser adicionado</param>
        Task Add(Cliente cliente);
        
        /// <summary>
        /// Atualiza um cliente existente no repositório
        /// </summary>
        /// <param name="cliente">Cliente a ser atualizado</param>
        Task Update(Cliente cliente);
        
        /// <summary>
        /// Busca um cliente pelo seu ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Cliente encontrado ou null</returns>
        Task<Cliente> GetById(Guid id);
        
        /// <summary>
        /// Lista todos os clientes cadastrados
        /// </summary>
        /// <returns>Lista de clientes</returns>
        Task<List<Cliente>> GetAll();
        
        /// <summary>
        /// Remove um cliente do repositório
        /// </summary>
        /// <param name="id">ID do cliente a ser removido</param>
        Task Delete(Guid id);
    }
}

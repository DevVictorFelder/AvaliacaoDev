using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaliacaoDev.Application.DTOs;

namespace AvaliacaoDev.Application.Interfaces
{
    /// <summary>
    /// Interface de serviço para gerenciamento de clientes.
    /// Define o contrato para casos de uso relacionados a clientes.
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Cria um novo cliente no sistema
        /// </summary>
        /// <param name="dto">Dados do cliente a ser criado</param>
        /// <returns>ID do cliente criado</returns>
        Task<Guid> Criar(ClienteDto dto);
        
        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <param name="dto">Dados atualizados do cliente</param>
        Task Atualizar(Guid id, ClienteDto dto);
        
        /// <summary>
        /// Remove um cliente do sistema
        /// </summary>
        /// <param name="id">ID do cliente a ser removido</param>
        Task Deletar(Guid id);
        
        /// <summary>
        /// Obtém um cliente específico pelo ID
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Dados completos do cliente</returns>
        Task<ClienteDto> Obter(Guid id);
        
        /// <summary>
        /// Lista todos os clientes cadastrados
        /// </summary>
        /// <param name="page">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <returns>Lista de clientes paginada</returns>
        Task<List<ClienteDto>> Listar(int page = 1, int pageSize = 10);
    }
}

using AvaliacaoDev.Application.DTOs;

namespace AvaliacaoDev.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<Guid> Criar(ProdutoDto dto);
        Task Atualizar(Guid id, ProdutoDto dto);
        Task Deletar(Guid id);
        Task<List<ProdutoDto>> Listar();
        Task<ProdutoDto> Obter(Guid id);
    }
}
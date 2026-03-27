using AvaliacaoDev.Domain.Entities;

namespace AvaliacaoDev.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task Add(Produto produto);
        Task Update(Produto produto);
        Task Delete(Guid id);
        Task<Produto> GetById(Guid id);
        Task<List<Produto>> GetAll();
    }
}
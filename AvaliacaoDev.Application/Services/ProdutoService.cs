using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;
using AvaliacaoDev.Domain.Entities;
using AvaliacaoDev.Domain.Interfaces;

namespace AvaliacaoDev.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Criar(ProdutoDto dto)
        {
            var produto = new Produto(dto.Nome, dto.Preco);
            await _repository.Add(produto);
            return produto.Id;
        }

        public async Task Atualizar(Guid id, ProdutoDto dto)
        {
            var produto = await _repository.GetById(id);

            if (produto == null)
                throw new Exception("Produto não encontrado");

            // Atualização simples (sem setter público)
            var atualizado = new Produto(dto.Nome, dto.Preco);

            typeof(Produto).GetProperty("Id")?.SetValue(atualizado, id);

            await _repository.Update(atualizado);
        }

        public async Task Deletar(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task<List<ProdutoDto>> Listar()
        {
            var produtos = await _repository.GetAll();

            return produtos.Select(p => new ProdutoDto
            {
                Nome = p.Nome,
                Preco = p.Preco
            }).ToList();
        }

        public async Task<ProdutoDto> Obter(Guid id)
        {
            var p = await _repository.GetById(id);

            return new ProdutoDto
            {
                Nome = p.Nome,
                Preco = p.Preco
            };
        }
    }
}
using AvaliacaoDev.Domain.Entities;
using AvaliacaoDev.Domain.Interfaces;
using System.Linq;
using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;

namespace AvaliacaoDev.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Criar(ClienteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new Exception("Nome é obrigatório");
            
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email é obrigatório");
            
            if (string.IsNullOrWhiteSpace(dto.CpfCnpj))
                throw new Exception("CPF/CNPJ é obrigatório");

            var cliente = new Cliente(dto.Nome, dto.CpfCnpj, dto.Email, dto.Telefone);

            await _repository.Add(cliente);

            return cliente.Id;
        }

        public async Task Atualizar(Guid id, ClienteDto dto)
        {
            var cliente = await _repository.GetById(id);

            if (cliente == null)
                throw new Exception("Cliente não encontrado");

            cliente.Atualizar(dto.Nome, dto.CpfCnpj, dto.Email, dto.Telefone);

            await _repository.Update(cliente);
        }

        public async Task<List<ClienteDto>> Listar(int page = 1, int pageSize = 10)
        {
            var clientes = await _repository.GetAll();
            
            var skip = (page - 1) * pageSize;
            var clientesPaginados = clientes.Skip(skip).Take(pageSize).ToList();

            return clientesPaginados.Select(c => new ClienteDto
            {
                Nome = c.Nome,
                CpfCnpj = c.CpfCnpj,
                Email = c.Email,
                Telefone = c.Telefone
            }).ToList();
        }

        public async Task Deletar(Guid id)
        {
            var cliente = await _repository.GetById(id);
            if (cliente == null)
                throw new Exception("Cliente não encontrado");
                
            // Verificar se cliente tem pedidos associados antes de deletar
            // (Implementar lógica de negócio se necessário)
            
            await _repository.Delete(id);
        }

        public async Task<ClienteDto> Obter(Guid id)
        {
            var cliente = await _repository.GetById(id);
            if (cliente == null)
                throw new Exception("Cliente não encontrado");
                
            return new ClienteDto
            {
                Nome = cliente.Nome,
                CpfCnpj = cliente.CpfCnpj,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaliacaoDev.Application.DTOs;
using AvaliacaoDev.Application.Interfaces;
using AvaliacaoDev.Domain.Entities;
using AvaliacaoDev.Domain.Interfaces;

namespace AvaliacaoDev.Application.Services
{
    // Orquestra os casos de uso
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        
        // Cache simples para evitar race condition do cliente padrão
        private static readonly object _lock = new object();
        private static Cliente? _clientePadraoCache;

        public PedidoService(IPedidoRepository repository, IClienteRepository clienteRepository, IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<Guid> CriarPedido(Guid? clienteId = null)
        {
            Cliente cliente;

            if (clienteId.HasValue)
            {
                // Se cliente foi informado, buscar no repositório
                cliente = await _clienteRepository.GetById(clienteId.Value);
                if (cliente == null)
                    throw new Exception($"Cliente com ID {clienteId.Value} não encontrado");
            }
            else
            {
                // Se nenhum cliente foi informado, usar o cliente padrão
                cliente = await ObterOuCriarClientePadrao();
            }

            var pedido = new Pedido(cliente);
            await _repository.Add(pedido);
            return pedido.Id;
        }

        private async Task<Cliente> ObterOuCriarClientePadrao()
        {
            // Verifica cache primeiro para não buscar toda hora
            if (_clientePadraoCache != null)
                return _clientePadraoCache;

            lock (_lock)
            {
                // Double-check pattern - verifica de novo depois do lock
                if (_clientePadraoCache != null)
                    return _clientePadraoCache;
            }

            // Busca cliente padrão existente
            var clientes = await _clienteRepository.GetAll();
            var clientePadrao = clientes.FirstOrDefault(c => 
                c.Nome.Equals("Consumidor Padrão", StringComparison.OrdinalIgnoreCase) &&
                c.Email.Equals("consumidor.padrao@exemplo.com", StringComparison.OrdinalIgnoreCase));

            if (clientePadrao == null)
            {
                lock (_lock)
                {
                    // Verifica novamente após obter o lock
                    if (_clientePadraoCache != null)
                        return _clientePadraoCache;

                    // Verifica duplicidade ANTES de criar
                    var clientesExistentes = _clienteRepository.GetAll().Result;
                    var temDuplicidade = clientesExistentes.Any(c => 
                        c.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "").Equals("52998224725") ||
                        c.Email.Equals("consumidor.padrao@exemplo.com", StringComparison.OrdinalIgnoreCase));

                    if (!temDuplicidade)
                    {
                        // Cria o cliente padrão
                        clientePadrao = new Cliente("Consumidor Padrão", "52998224725", "consumidor.padrao@exemplo.com", "11999999999");
                        _clienteRepository.Add(clientePadrao).Wait();
                        _clientePadraoCache = clientePadrao;
                    }
                    else
                    {
                        // Se encontrou duplicidade, usa o existente
                        clientePadrao = clientesExistentes.First(c => 
                            c.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "").Equals("52998224725") ||
                            c.Email.Equals("consumidor.padrao@exemplo.com", StringComparison.OrdinalIgnoreCase));
                        _clientePadraoCache = clientePadrao;
                    }
                }
            }
            else
            {
                lock (_lock)
                {
                    _clientePadraoCache = clientePadrao;
                }
            }

            return clientePadrao;
        }

        private async Task<bool> VerificarDuplicidadeCliente(Cliente cliente)
        {
            var clientes = await _clienteRepository.GetAll();
            
            return clientes.Any(c => 
                (c.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "").Equals(
                    cliente.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""), 
                    StringComparison.OrdinalIgnoreCase)) ||
                c.Email.Equals(cliente.Email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AdicionarProduto(Guid pedidoId, ProdutoDto dto)
        {
            var pedido = await _repository.GetById(pedidoId);

            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new Exception("Nome do produto é obrigatório");

            if (dto.Preco <= 0)
                throw new Exception("Preço do produto deve ser maior que zero");

            if (dto.Quantidade <= 0)
                throw new Exception("Quantidade deve ser maior que zero");

            // Normaliza nome para busca (remove espaços extras e converte para minúsculas)
            var nomeNormalizado = dto.Nome.Trim().ToLowerInvariant();

            // Busca produto existente de forma mais eficiente
            var produtoExistente = await BuscarProdutoPorNomePreco(nomeNormalizado, dto.Preco);

            Produto produto;

            if (produtoExistente != null)
            {
                produto = produtoExistente;
            }
            else
            {
                // Verifica duplicidade antes de criar novo produto
                if (await VerificarDuplicidadeProduto(nomeNormalizado, dto.Preco))
                {
                    throw new Exception("Produto com mesmo nome e preço já existe");
                }

                // Cria novo produto
                produto = new Produto(dto.Nome.Trim(), dto.Preco);
                await _produtoRepository.Add(produto);
            }

            pedido.AdicionarProduto(produto, dto.Quantidade);

            await _repository.Update(pedido);
        }

        private async Task<Produto?> BuscarProdutoPorNomePreco(string nomeNormalizado, decimal preco)
        {
            var produtos = await _produtoRepository.GetAll();
            
            return produtos.FirstOrDefault(p => 
                p.Nome.Trim().ToLowerInvariant().Equals(nomeNormalizado) && 
                p.Preco == preco);
        }

        private async Task<bool> VerificarDuplicidadeProduto(string nomeNormalizado, decimal preco)
        {
            var produtos = await _produtoRepository.GetAll();
            
            return produtos.Any(p => 
                p.Nome.Trim().ToLowerInvariant().Equals(nomeNormalizado) && 
                p.Preco == preco);
        }

        public async Task RemoverProduto(Guid pedidoId, Guid produtoId)
        {
            var pedido = await _repository.GetById(pedidoId);

            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            pedido.RemoverProduto(produtoId);

            await _repository.Update(pedido);
        }

        public async Task FecharPedido(Guid pedidoId)
        {
            var pedido = await _repository.GetById(pedidoId);

            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            pedido.FecharPedido();

            await _repository.Update(pedido);
        }

        public async Task<List<PedidoDto>> Listar(int page, int pageSize, string status)
        {
            var pedidos = await _repository.GetAll(page, pageSize, status);

            return pedidos.Select(p => new PedidoDto
            {
                Id = p.Id,
                Status = p.Status.ToString(),
                Produtos = p.Produtos?.Select(prod => new ProdutoDto
                {
                    Nome = prod.Nome,
                    Preco = prod.Preco,
                    Quantidade = 1 // Quantidade padrão para exibição
                }).ToList() ?? new List<ProdutoDto>(),
                Cliente = p.Cliente != null ? new ClienteDto
                {
                    Nome = p.Cliente.Nome,
                    CpfCnpj = p.Cliente.CpfCnpj,
                    Email = p.Cliente.Email,
                    Telefone = p.Cliente.Telefone
                } : new ClienteDto()
            }).ToList();
        }

        public async Task<PedidoDto> Obter(Guid id)
        {
            var pedido = await _repository.GetById(id);

            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            return new PedidoDto
            {
                Id = pedido.Id,
                Status = pedido.Status.ToString(),
                Produtos = pedido.Produtos?.Select(prod => new ProdutoDto
                {
                    Nome = prod.Nome,
                    Preco = prod.Preco,
                    Quantidade = 1 // Quantidade padrão para exibição
                }).ToList() ?? new List<ProdutoDto>(),
                Cliente = pedido.Cliente != null ? new ClienteDto
                {
                    Nome = pedido.Cliente.Nome,
                    CpfCnpj = pedido.Cliente.CpfCnpj,
                    Email = pedido.Cliente.Email,
                    Telefone = pedido.Cliente.Telefone
                } : new ClienteDto()
            };
        }
    }
}
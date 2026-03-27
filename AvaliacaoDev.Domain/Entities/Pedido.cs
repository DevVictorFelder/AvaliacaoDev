using System;
using System.Collections.Generic;
using System.Linq;
using AvaliacaoDev.Domain.Enums;

namespace AvaliacaoDev.Domain.Entities
{
    /// <summary>
    /// Aggregate Root principal do domínio.
    /// Representa um pedido de vendas com seus itens e regras de negócio.
    /// </summary>
    public class Pedido
    {
        /// <summary>
        /// Identificador único do pedido
        /// </summary>
        public Guid Id { get; private set; }
        
        /// <summary>
        /// Lista de produtos associados ao pedido
        /// </summary>
        public List<Produto> Produtos { get; private set; } = new();
        
        /// <summary>
        /// Status atual do pedido (Aberto/Fechado)
        /// </summary>
        public StatusPedido Status { get; private set; }
        
        /// <summary>
        /// Cliente associado ao pedido
        /// </summary>
        public Cliente Cliente { get; private set; }
        
        /// <summary>
        /// Construtor sem parâmetros para EF Core
        /// </summary>
        private Pedido() { }
        
        /// <summary>
        /// Construtor que cria um novo pedido associado a um cliente
        /// </summary>
        /// <param name="cliente">Cliente que está realizando o pedido</param>
        public Pedido(Cliente cliente)
        {
            Id = Guid.NewGuid();
            Status = StatusPedido.Aberto;
            Cliente = cliente;
        }

        /// <summary>
        /// Adiciona um produto ao pedido
        /// </summary>
        /// <param name="produto">Produto a ser adicionado</param>
        /// <param name="quantidade">Quantidade do produto (padrão: 1)</param>
        /// <exception cref="Exception">Lançada se o pedido estiver fechado</exception>
        public void AdicionarProduto(Produto produto, int quantidade = 1)
        {
            // Não pode adicionar produto se o pedido já estiver fechado
            if (Status == StatusPedido.Fechado)
                throw new Exception("Pedido já está fechado");

            // Quantidade tem que ser maior que zero
            if (quantidade <= 0)
                throw new Exception("Quantidade deve ser maior que zero");

            // Remove o produto se já existir para adicionar com a nova quantidade
            var produtoExistente = Produtos.FirstOrDefault(p => p.Id == produto.Id);
            if (produtoExistente != null)
            {
                Produtos.Remove(produtoExistente);
            }

            // Adiciona o produto ao pedido
            Produtos.Add(produto);
        }

        /// <summary>
        /// Remove um produto do pedido pelo ID
        /// </summary>
        /// <param name="produtoId">ID do produto a ser removido</param>
        /// <exception cref="Exception">Lançada se o pedido estiver fechado</exception>
        public void RemoverProduto(Guid produtoId)
        {
            // Não pode remover produto se o pedido já estiver fechado
            if (Status == StatusPedido.Fechado)
                throw new Exception("Pedido já está fechado");

            var produto = Produtos.FirstOrDefault(p => p.Id == produtoId);

            if (produto != null)
                Produtos.Remove(produto);
        }

        /// <summary>
        /// Fecha o pedido, impedindo novas alterações
        /// </summary>
        /// <exception cref="Exception">Lançada se o pedido não tiver produtos</exception>
        public void FecharPedido()
        {
            // Pedido precisa ter pelo menos um produto para ser fechado
            if (!Produtos.Any())
                throw new Exception("Pedido precisa ter ao menos um produto");

            Status = StatusPedido.Fechado;
        }
    }
}
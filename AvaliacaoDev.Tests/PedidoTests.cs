using AvaliacaoDev.Domain.Entities;
using Xunit;

namespace AvaliacaoDev.Tests
{
    public class PedidoTests
    {
        [Fact]
        public void Nao_Deve_Fechar_Pedido_Sem_Produtos()
        {
            var cliente = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var pedido = new Pedido(cliente);

            Assert.Throws<Exception>(() => pedido.FecharPedido());
        }

        [Fact]
        public void Deve_Adicionar_Produto_Com_Quantidade()
        {
            var cliente = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var pedido = new Pedido(cliente);
            var produto = new Produto("Teste", 10);

            pedido.AdicionarProduto(produto, 2);

            Assert.Single(pedido.Produtos);
        }

        [Fact]
        public void Pedido_Deve_Ser_Criado_Com_Cliente_Obrigatoriamente()
        {
            // Verifica se o construtor do Pedido exige um cliente
            var cliente = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var pedido = new Pedido(cliente);

            Assert.NotNull(pedido.Cliente);
            Assert.Equal(cliente.Id, pedido.Cliente.Id);
            Assert.Equal("Teste", pedido.Cliente.Nome);
        }

        [Fact]
        public void Cliente_Padrao_Deve_Ter_Dados_Validos()
        {
            // Verifica se o cliente padrão pode ser criado com dados válidos
            var clientePadrao = new Cliente("Consumidor Padrão", "52998224725", "consumidor.padrao@exemplo.com", "11999999999");

            Assert.Equal("Consumidor Padrão", clientePadrao.Nome);
            Assert.Equal("52998224725", clientePadrao.CpfCnpj);
            Assert.Equal("consumidor.padrao@exemplo.com", clientePadrao.Email);
            Assert.Equal("11999999999", clientePadrao.Telefone);
        }

        [Fact]
        public void Deve_Validar_Email_Invalido()
        {
            Assert.Throws<Exception>(() => new Cliente("Teste", "52998224725", "email-invalido", "11999999999"));
        }

        [Fact]
        public void Deve_Validar_Telefone_Invalido()
        {
            Assert.Throws<Exception>(() => new Cliente("Teste", "52998224725", "teste@email.com", "123"));
        }

        [Fact]
        public void Deve_Validar_Telefone_Com_Formato_Brasileiro()
        {
            // Deve aceitar formatos válidos
            var cliente1 = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var cliente2 = new Cliente("Teste", "52998224725", "teste@email.com", "(11) 99999-9999");
            
            Assert.NotNull(cliente1);
            Assert.NotNull(cliente2);
        }

        [Fact]
        public void Produto_Deve_Ser_Criado_Com_Dados_Validos()
        {
            var produto = new Produto("Teste", 10);
            Assert.Equal("Teste", produto.Nome);
            Assert.Equal(10, produto.Preco);
        }

        [Fact]
        public void Nao_Deve_Criar_Produto_Com_Preco_Invalido()
        {
            Assert.Throws<Exception>(() => new Produto("Teste", -1));
            Assert.Throws<Exception>(() => new Produto("Teste", 0));
        }

        [Fact]
        public void Nao_Deve_Adicionar_Produto_Com_Quantidade_Invalida()
        {
            var cliente = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var pedido = new Pedido(cliente);
            var produto = new Produto("Teste", 10);

            Assert.Throws<Exception>(() => pedido.AdicionarProduto(produto, 0));
            Assert.Throws<Exception>(() => pedido.AdicionarProduto(produto, -1));
        }

        [Fact]
        public void Nao_Deve_Adicionar_Produto_Pedido_Fechado()
        {
            var cliente = new Cliente("Teste", "52998224725", "teste@email.com", "11999999999");
            var pedido = new Pedido(cliente);
            var produto = new Produto("Teste", 10);
            
            // Adiciona um produto e fecha o pedido
            pedido.AdicionarProduto(produto);
            pedido.FecharPedido();

            // Tenta adicionar outro produto
            Assert.Throws<Exception>(() => pedido.AdicionarProduto(produto));
        }
    }
}
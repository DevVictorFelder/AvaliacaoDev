namespace AvaliacaoDev.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object para transferência de dados de pedidos.
    /// Usado para retorno de dados na API com informações completas.
    /// </summary>
    public class PedidoDto
    {
        /// <summary>
        /// Identificador único do pedido
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Status atual do pedido
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Lista de produtos do pedido
        /// </summary>
        public List<ProdutoDto> Produtos { get; set; } = new();
        
        /// <summary>
        /// Dados do cliente associado ao pedido
        /// </summary>
        public ClienteDto Cliente { get; set; } = new();
    }
}
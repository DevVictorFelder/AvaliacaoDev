namespace AvaliacaoDev.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object para transferência de dados de produtos.
    /// Usado para entrada e saída de dados na API.
    /// </summary>
    public class ProdutoDto
    {
        /// <summary>
        /// Nome do produto
        /// </summary>
        /// <example>Notebook Dell</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Nome do produto é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(200, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// Preço unitário do produto
        /// </summary>
        /// <example>3500.50</example>
        [System.ComponentModel.DataAnnotations.Range(0.01, 999999.99, ErrorMessage = "Preço deve ser maior que 0 e menor que R$ 999.999,99")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Quantidade desejada para adicionar ao pedido
        /// </summary>
        /// <example>2</example>
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantidade { get; set; } = 1;
    }
}
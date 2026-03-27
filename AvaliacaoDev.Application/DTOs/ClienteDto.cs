namespace AvaliacaoDev.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object para transferência de dados de clientes.
    /// Usado para entrada e saída de dados na API.
    /// </summary>
    public class ClienteDto
    {
        /// <summary>
        /// Nome completo do cliente
        /// </summary>
        /// <example>João Silva</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Nome é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// CPF ou CNPJ do cliente
        /// </summary>
        /// <example>12345678901</example>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "CPF/CNPJ é obrigatório")]
        [System.ComponentModel.DataAnnotations.StringLength(14, MinimumLength = 11, ErrorMessage = "CPF/CNPJ deve ter entre 11 e 14 caracteres")]
        public string CpfCnpj { get; set; } = string.Empty;
        
        /// <summary>
        /// Email do cliente
        /// </summary>
        /// <example>joao@email.com</example>
        [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Telefone do cliente
        /// </summary>
        /// <example>11999999999</example>
        [System.ComponentModel.DataAnnotations.Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; } = string.Empty;
    }
}
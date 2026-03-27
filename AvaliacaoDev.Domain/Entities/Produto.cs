using System;

namespace AvaliacaoDev.Domain.Entities
{
    /// <summary>
    /// Entidade que representa um produto no sistema.
    /// Contém informações básicas como nome, preço e identificador único.
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// Identificador único do produto (GUID)
        /// </summary>
        public Guid Id { get; private set; }
        
        /// <summary>
        /// Nome descritivo do produto
        /// </summary>
        public string Nome { get; private set; }
        
        /// <summary>
        /// Preço unitário do produto em formato decimal
        /// </summary>
        public decimal Preco { get; private set; }

        /// <summary>
        /// Construtor da entidade Produto
        /// </summary>
        /// <param name="nome">Nome do produto</param>
        /// <param name="preco">Preço do produto</param>
        public Produto(string nome, decimal preco)
        {
            // Valida antes de criar o produto
            Validar(nome, preco);
            
            Id = Guid.NewGuid();
            Nome = nome;
            Preco = preco;
        }

        private void Validar(string nome, decimal preco)
        {
            // Nome não pode ser vazio
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Nome do produto é obrigatório");

            // Limita tamanho do nome
            if (nome.Length > 200)
                throw new Exception("Nome do produto não pode exceder 200 caracteres");

            // Preço tem que ser positivo
            if (preco <= 0)
                throw new Exception("Preço do produto deve ser maior que zero");

            // Limita valor máximo do preço
            if (preco > 999999.99m)
                throw new Exception("Preço do produto não pode exceder R$ 999.999,99");
        }
    }
}
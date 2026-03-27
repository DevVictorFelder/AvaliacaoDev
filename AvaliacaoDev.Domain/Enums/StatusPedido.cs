namespace AvaliacaoDev.Domain.Enums
{
    /// <summary>
    /// Enumeração que representa os possíveis status de um pedido no sistema.
    /// Controla o fluxo do ciclo de vida do pedido.
    /// </summary>
    public enum StatusPedido
    {
        /// <summary>
        /// Pedido recém-criado, aberto para alterações
        /// </summary>
        Aberto,
        
        /// <summary>
        /// Pedido finalizado, não permite mais alterações
        /// </summary>
        Fechado
    }
}
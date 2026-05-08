namespace ContaBancaria.Models
{
    /// <summary>
    /// Define os tipos de contas bancárias suportadas pelo sistema.
    /// </summary>
    public enum TipoConta
    {
        /// <summary>
        /// Representa uma conta do tipo corrente, com suporte a limite de crédito.
        /// </summary>
        Corrente = 1,

        /// <summary>
        /// Representa uma conta do tipo poupança, com suporte a rendimentos por aniversário.
        /// </summary>
        Poupanca = 2
    }
}
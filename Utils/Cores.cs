namespace ContaBancaria.Utils
{
    /// <summary>
    /// Utilitário para formatação de cores no console.
    /// Aumenta a visibilidade e a legibilidade da interface de utilizador.
    /// </summary>
    public class Cores
    {
        /// <summary>Define a cor do texto para vermelho.</summary>
        public const string CorTextoVermelho = "\u001b[31m";

        /// <summary>Define a cor do texto para verde.</summary>
        public const string CorTextoVerde = "\u001b[32m";

        /// <summary>Define a cor do texto para amarelo.</summary>
        public const string CorTextoAmarelo = "\u001b[33m";

        /// <summary>Define a cor do texto para azul.</summary>
        public const string CorTextoAzul = "\u001b[34m";

        /// <summary>Define a cor do texto para branco.</summary>
        public const string CorTextoBranco = "\u001b[37m";

        /// <summary>Restaura a cor do texto para o padrão do terminal.</summary>
        public const string CorReset = "\u001b[0m";
    }
}
using System;
using System.Linq;

namespace ContaBancaria.Utils
{
    /// <summary>
    /// Classe estática utilitária para leitura e validação de entradas do utilizador via console.
    /// Centraliza os ciclos de validação para reduzir a repetição de código na camada de apresentação.
    /// </summary>
    public static class InputHelper
    {
        /// <summary>
        /// Lê um número inteiro válido do console através de um ciclo de revalidação.
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: valor maior que zero.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo o valor inteiro lido e um booleano indicando se a operação foi cancelada.</returns>
        public static (int valor, bool cancelou) LerInteiro(string mensagem, string msgErro, Func<int, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= v => v > 0;

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (0, true);

                if (int.TryParse(input, out int valor) && validacao(valor))
                    return (valor, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }

        /// <summary>
        /// Lê um valor decimal válido do console através de um ciclo de revalidação.
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: valor maior que zero.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo o valor decimal lido e um booleano indicando se a operação foi cancelada.</returns>
        public static (decimal valor, bool cancelou) LerDecimal(string mensagem, string msgErro, Func<decimal, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= v => v > 0;

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (0m, true);

                if (decimal.TryParse(input, out decimal valor) && validacao(valor))
                    return (valor, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }
        
        /// <summary>
        /// Lê uma string não-vazia do console através de um ciclo de revalidação.
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: string não vazia ou com espaços em branco.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo a string lida e um booleano indicando se a operação foi cancelada.</returns>
        public static (string valor, bool cancelou) LerString(string mensagem, string msgErro, Func<string, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= s => !string.IsNullOrWhiteSpace(s);

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (string.Empty, true);

                if (validacao(input))
                    return (input, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }

        /// <summary>
        /// Lê um valor inteiro do console ou mantém o valor atual se o utilizador submeter uma entrada vazia (Enter).
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="valorPadrao">O valor de fallback a ser retornado caso a entrada seja vazia.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: valor maior que zero.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo o valor inteiro processado e um booleano indicando cancelamento.</returns>
        public static (int valor, bool cancelou) LerInteiroOuManter(string mensagem, string msgErro, int valorPadrao, Func<int, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= v => v > 0;

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (0, true);
                if (string.IsNullOrWhiteSpace(input)) return (valorPadrao, false);

                if (int.TryParse(input, out int valor) && validacao(valor))
                    return (valor, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }

        /// <summary>
        /// Lê uma string do console ou mantém o valor atual se o utilizador submeter uma entrada vazia (Enter).
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="valorPadrao">O valor de fallback a ser retornado caso a entrada seja vazia.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: string não vazia ou com espaços em branco.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo a string processada e um booleano indicando cancelamento.</returns>
        public static (string valor, bool cancelou) LerStringOuManter(string mensagem, string msgErro, string valorPadrao, Func<string, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= s => !string.IsNullOrWhiteSpace(s);

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (string.Empty, true);
                if (string.IsNullOrWhiteSpace(input)) return (valorPadrao, false);

                if (validacao(input))
                    return (input, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }

        /// <summary>
        /// Lê um valor decimal do console ou mantém o valor atual se o utilizador submeter uma entrada vazia (Enter).
        /// </summary>
        /// <param name="mensagem">Texto exibido ao utilizador antes da leitura.</param>
        /// <param name="msgErro">Mensagem exibida quando a entrada falha na validação.</param>
        /// <param name="valorPadrao">O valor de fallback a ser retornado caso a entrada seja vazia.</param>
        /// <param name="validacao">Predicado opcional de validação extra. Por omissão: valor maior ou igual a zero.</param>
        /// <param name="cancelToken">Texto que o utilizador pode digitar para cancelar a operação. Por omissão: "0".</param>
        /// <returns>Uma tupla contendo o valor decimal processado e um booleano indicando cancelamento.</returns>
        public static (decimal valor, bool cancelou) LerDecimalOuManter(string mensagem, string msgErro, decimal valorPadrao, Func<decimal, bool>? validacao = null, string cancelToken = "0")
        {
            validacao ??= v => v >= 0;

            while (true)
            {
                Console.Write(mensagem);
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Trim() == cancelToken) return (0m, true);
                if (string.IsNullOrWhiteSpace(input)) return (valorPadrao, false);

                if (decimal.TryParse(input, out decimal valor) && validacao(valor))
                    return (valor, false);

                Console.WriteLine(Cores.CorTextoVermelho + msgErro + Cores.CorReset + "\n");
            }
        }
    }
}
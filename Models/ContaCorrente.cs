using System;

namespace ContaBancaria.Models
{
    /// <summary>
    /// Representa uma conta bancária do tipo Corrente.
    /// Esta classe estende a funcionalidade base de <see cref="Conta"/> ao introduzir um limite de crédito.
    /// </summary>
    public class ContaCorrente : Conta
    {
        /// <summary>
        /// Obtém ou define o limite de crédito disponível para saques além do saldo real.
        /// </summary>
        public decimal Limite { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContaCorrente"/>.
        /// </summary>
        /// <param name="numero">Número identificador da conta.</param>
        /// <param name="agencia">Número da agência vinculada.</param>
        /// <param name="tipo">Tipo da conta conforme o enumerador <see cref="TipoConta"/>.</param>
        /// <param name="titular">Nome completo do titular da conta.</param>
        /// <param name="saldo">Saldo inicial disponível.</param>
        /// <param name="limite">Limite de crédito concedido à conta.</param>
        public ContaCorrente(int numero, int agencia, TipoConta tipo, string titular, decimal saldo, decimal limite) 
            : base(numero, agencia, tipo, titular, saldo)
        {
            Limite = limite;
        }

        /// <summary>
        /// Realiza o débito de um valor considerando o saldo disponível somado ao limite de crédito.
        /// </summary>
        /// <param name="valor">Montante a ser sacado.</param>
        /// <returns>True se o montante total (saldo + limite) for suficiente; caso contrário, False.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor informado é menor ou igual a zero.</exception>
        public override bool Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor do saque deve ser maior que zero.");
            }

            if ((this.Saldo + this.Limite) < valor)
            {
                Console.WriteLine("\nSaldo insuficiente, mesmo considerando o limite de crédito.");
                return false;
            }

            this.Saldo -= valor;
            return true;
        }

        /// <summary>
        /// Exibe os dados da conta corrente, incluindo o limite de crédito configurado.
        /// </summary>
        public override void Visualizar()
        {
            base.Visualizar(); 
            Console.WriteLine($"Limite de Crédito: {this.Limite:C}");
        }
    }
}
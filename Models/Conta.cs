using System;
using System.Text.Json.Serialization;

namespace ContaBancaria.Models
{
    /// <summary>
    /// Atributos de serialização polimórfica para garantir a correta identificação das subclasses durante a persistência em JSON.
    /// </summary>
    [JsonDerivedType(typeof(ContaCorrente), typeDiscriminator: "corrente")]
    [JsonDerivedType(typeof(ContaPoupanca), typeDiscriminator: "poupanca")]

    /// <summary>
    /// Representa a estrutura base para contas bancárias. 
    /// Classe abstrata que define os comportamentos fundamentais de movimentação financeira.
    /// </summary>
    public abstract class Conta
    {
        public int Numero { get; set; }
        public int Agencia { get; set; }
        public TipoConta Tipo { get; set; }
        public string Titular { get; set; }
        public decimal Saldo { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Conta"/>.
        /// </summary>
        /// <param name="numero">Número identificador da conta.</param>
        /// <param name="agencia">Número da agência vinculada.</param>
        /// <param name="tipo">Tipo da conta conforme o enumerador <see cref="TipoConta"/>.</param>
        /// <param name="titular">Nome completo do titular da conta.</param>
        /// <param name="saldo">Saldo inicial disponível para movimentação.</param>
        public Conta(int numero, int agencia, TipoConta tipo, string titular, decimal saldo)
        {
            Numero = numero;
            Agencia = agencia;
            Tipo = tipo;
            Titular = titular;
            Saldo = saldo;
        }

        /// <summary>
        /// Realiza o débito de um valor no saldo da conta, verificando a disponibilidade de fundos.
        /// </summary>
        /// <param name="valor">Montante a ser sacado.</param>
        /// <returns>True se a operação for bem-sucedida; caso contrário, False.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor informado é menor ou igual a zero.</exception>
        public virtual bool Sacar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor do saque deve ser maior que zero.");
            }

            if (this.Saldo < valor)
            {
                Console.WriteLine("\nSaldo insuficiente para realizar a operação.");
                return false;
            }

            this.Saldo -= valor;
            return true;
        }

        /// <summary>
        /// Realiza o depósito de um valor no saldo da conta.
        /// </summary>
        /// <param name="valor">Montante a ser depositado.</param>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor informado é menor ou igual a zero.</exception>
        public virtual void Depositar(decimal valor)
        {
            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor do depósito deve ser maior que zero.");
            }

            this.Saldo += valor;
        }

        /// <summary>
        /// Exibe os dados detalhados da conta no console.
        /// </summary>
        public virtual void Visualizar()
        {
            Console.WriteLine("\n\n***********************************************************");
            Console.WriteLine("DETALHES DA CONTA");
            Console.WriteLine("***********************************************************");
            Console.WriteLine($"Número: {this.Numero}");
            Console.WriteLine($"Agência: {this.Agencia}");
            Console.WriteLine($"Tipo: {this.Tipo}");
            Console.WriteLine($"Titular: {this.Titular}");
            Console.WriteLine($"Saldo: {this.Saldo:C}");
        }
    }
}
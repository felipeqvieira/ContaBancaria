using System;

namespace ContaBancaria.Models
{
    /// <summary>
    /// Representa uma conta bancária do tipo Poupança.
    /// Aplica rendimentos automáticos sobre o saldo caso o depósito ocorra no dia do aniversário da conta.
    /// </summary>
    public class ContaPoupanca : Conta
    {
        /// <summary>
        /// Obtém ou define o dia do mês (1 a 28) em que a conta completa aniversário para fins de rendimento.
        /// </summary>
        public int Aniversario { get; set; }

        /// <summary>
        /// Armazena a data da última vez em que o rendimento de juros foi aplicado para evitar duplicidade no mesmo mês.
        /// </summary>
        public DateTime UltimoRendimento { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContaPoupanca"/>.
        /// </summary>
        /// <param name="numero">Número identificador da conta.</param>
        /// <param name="agencia">Número da agência vinculada.</param>
        /// <param name="tipo">Tipo da conta conforme o enumerador <see cref="TipoConta"/>.</param>
        /// <param name="titular">Nome completo do titular da conta.</param>
        /// <param name="saldo">Saldo inicial disponível.</param>
        /// <param name="aniversario">Dia do aniversário da conta.</param>
        public ContaPoupanca(int numero, int agencia, TipoConta tipo, string titular, decimal saldo, int aniversario) 
            : base(numero, agencia, tipo, titular, saldo)
        {
            Aniversario = aniversario;
        }

        /// <summary>
        /// Realiza o depósito de um valor e aplica um rendimento de 0,5% sobre o saldo caso hoje seja o dia de aniversário.
        /// </summary>
        /// <param name="valor">Montante a ser depositado.</param>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor informado é menor ou igual a zero.</exception>
        public override void Depositar(decimal valor)
        {
            DateTime hoje = DateTime.Now;

            // se o dia do aniversário for hoje
            if (hoje.Day == this.Aniversario)
            {
                //valida possibilidade de ser o mesmo mes, mas em ano diferente
                if (UltimoRendimento.Month != hoje.Month || UltimoRendimento.Year != hoje.Year)
                {
                    decimal juros = this.Saldo * 0.005m;
                    this.Saldo += juros;
                    UltimoRendimento = hoje;

                    Console.WriteLine($"\n[RENDIMENTO] Dia de aniversário da conta identificado. Rendimento de {juros:C} aplicado ao saldo.");
                } 
            }

            base.Depositar(valor);
        }

        /// <summary>
        /// Exibe os dados da conta poupança, incluindo o dia configurado para aniversário.
        /// </summary>
        public override void Visualizar()
        {
            base.Visualizar();
            Console.WriteLine($"Dia de Aniversário: {this.Aniversario}");
        }
    }
}
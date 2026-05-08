using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public class ContaPoupanca : Conta
    {
        public int Aniversario {get; set;}
        public DateTime UltimoRendimento {get;set;} = DateTime.MinValue;

        public ContaPoupanca(int numero, int agencia, TipoConta tipo, string titular, decimal saldo, int aniversario) : base(numero, agencia, tipo, titular, saldo)
        {
            Aniversario = aniversario;
        }

        // Sobrescreve para utilizar aniversário. Caso seja o dia do aniversário, adiciona juros no saldo da conta uma vez.
        public override void Depositar(decimal valor)
        {

            DateTime hoje = DateTime.Now;

            // Verifica se hoje é o dia do aniversário
            if (hoje.Day == this.Aniversario)
            {
                // Verifica se não há repetição da atribuição do juros
                if (UltimoRendimento.Month != hoje.Month || UltimoRendimento.Year != hoje.Year)
                {
                    decimal juros = this.Saldo * 0.005m;
                    
                    this.Saldo += juros;
                    
                    UltimoRendimento = hoje;

                    Console.WriteLine($"Aniversário da conta! Depósito de {valor} realizado. Saldo total: {this.Saldo}");
                } 

                //adiciona valor no saldo da conta
                base.Depositar(valor);
            }
        }

        public override void Visualizar()
        {
            base.Visualizar();
            Console.WriteLine($"Aniversário da Conta: Dia {this.Aniversario}");
        }
    }
}
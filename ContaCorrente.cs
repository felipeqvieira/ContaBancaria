using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public class ContaCorrente : Conta
    {
        public decimal Limite{get; set;}

        public ContaCorrente(int numero, int agencia, TipoConta tipo, string titular, decimal saldo, decimal limite) : base(numero, agencia, tipo, titular, saldo)
        {
            //Só preciso declarar limite já que o resto tem no construtor base
            Limite = limite;
        }

        // Sobrescrever por conta do limite
        public override bool Sacar(decimal valor)
        {
            // Se saldo + Limite é menor que valor
            if((this.Saldo + this.Limite) < valor)
            {
                //saldo insuficiente
                Console.WriteLine("\nSaldo Insuficiente!");
                return false;
            }
            // caso contrário, retira valor do saldo
            this.Saldo -= valor;
            return true;
        }

        // Adiciona informação do limite na visualização padrão
        public override void Visualizar()
        {
            base.Visualizar(); 
            Console.WriteLine($"Limite: {this.Limite.ToString("c")}");
        }
    }
}
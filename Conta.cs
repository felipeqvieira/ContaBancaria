using System;

namespace ContaBancaria
{
    public abstract class Conta
    {
        //Atributos usando Properties
        public int Numero {get; set;}
        public int Agencia {get; set;}
        public TipoConta Tipo {get; set;} // troquei para um Enum p/ melhor visualização
        public string Titular {get; set;}
        public decimal Saldo {get; set;} // alterado para decimal já por ser monetário

        public Conta(int numero, int agencia, TipoConta tipo, string titular, decimal saldo)
        {
            Numero = numero;
            Agencia = agencia;
            Tipo = tipo;
            Titular = titular;
            Saldo = saldo;
        }

        // Retorna true se deu certo, false se não tem saldo
        public virtual bool Sacar(decimal valor)
        {

            if (valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor do saque deve ser maior que zero.");
            }

            if(this.Saldo < valor)
            {
                Console.WriteLine("\nSaldo insuficiente!");
                return false;
            }

            this.Saldo -= valor;
            return true;
        }
        
        // Soma o valor no saldo
        public virtual void Depositar(decimal valor)
        {

            //verifica argumento valor > 0
            if(valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor do depósito deve ser maior que zero.");
            }

            this.Saldo += valor;
        }
        
        public virtual void Visualizar()
        {
            Console.WriteLine("\n\n***********************************************************");
            Console.WriteLine("Dados da Conta:");
            Console.WriteLine("***********************************************************");
            Console.WriteLine($"Número da Conta: {this.Numero}");
            Console.WriteLine($"Agência: {this.Agencia}");
            Console.WriteLine($"Tipo da Conta: {this.Tipo}");
            Console.WriteLine($"Titular: {this.Titular}");
            Console.WriteLine($"Saldo: {this.Saldo.ToString("C")}"); // Formata como moeda local

        }
    }
}
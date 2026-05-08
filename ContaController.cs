using System;
using System.Collections.Generic;
using ContaBancaria;

public class ContaController : IContaRepository
{
    // Lista Genérica de contas
    private List<Conta> listaContas = new List<Conta>();
    
    // Número da conta que é incrementado a cada criação
    private int numeroId = 0;

    // Privado pois só a classe deve usar
    private int GerarNumero()
    {
        numeroId++;
        return numeroId;
    }

    // Busca conta na lista
    public Conta? BuscarNaCollection(int numero)
    {
        foreach (var conta in listaContas)
        {
            if (conta.Numero == numero)
                return conta;
        }
        return null; // Retorna null se não encontrar
    }

    //Adiciona conta na lista
    public void Cadastrar(Conta conta)
    {
        // Atribui o número gerado antes de salvar
        conta.Numero = GerarNumero();
        listaContas.Add(conta);
        Console.WriteLine($"\nA Conta número {conta.Numero} foi criada com sucesso!");
    }

    // Mostra todas as contas na lista
    public void ListarTodas()
    {
        if (listaContas.Count == 0)
        {
            Console.WriteLine("\nNão existem contas cadastradas no sistema.");
            return;
        }

        foreach (var conta in listaContas)
        {
            conta.Visualizar();
        }
    }

    // Procura conta pelo número
    public void ProcurarPorNumero(int numero)
    {
        Conta? conta = BuscarNaCollection(numero);

        if (conta != null)
            conta.Visualizar();
        else
            Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
    }

    // Atualiza dados da conta
    public void Atualizar(Conta contaAtualizada)
    {
        Conta? contaBuscada = BuscarNaCollection(contaAtualizada.Numero);

        if (contaBuscada != null)
        {
            // Atualiza os dados da conta na lista
            contaBuscada.Agencia = contaAtualizada.Agencia;
            contaBuscada.Tipo = contaAtualizada.Tipo;
            contaBuscada.Titular = contaAtualizada.Titular;
            Console.WriteLine($"\nA Conta número {contaAtualizada.Numero} foi atualizada com sucesso!");
        }
        else
            Console.WriteLine($"\nA Conta número {contaAtualizada.Numero} não foi encontrada!");
    }

    // Remove conta da lista
    public void Deletar(int numero)
    {
        Conta? contaBuscada = BuscarNaCollection(numero);

        if (contaBuscada != null)
        {
            if(listaContas.Remove(contaBuscada))
                 Console.WriteLine($"\nA Conta número {numero} foi deletada com sucesso!");
        }
        else
            Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
    }

    // Deposita valor na conta
    public void Depositar(int numero, decimal valor)
    {
        Conta? contaBuscada = BuscarNaCollection(numero);

        if (contaBuscada != null)
        {
            contaBuscada.Depositar(valor);
            Console.WriteLine($"\nDepósito de {valor.ToString("C")} efetuado com sucesso!");
        }
        else
            Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
    }

    // Saca valor da conta
    public void Sacar(int numero, decimal valor)
    {
        Conta? contaBuscada = BuscarNaCollection(numero);

        if (contaBuscada != null)
        {
            // Verifica se saque é possível
            if (contaBuscada.Sacar(valor) == true)
                Console.WriteLine($"\nSaque de {valor.ToString("C")} efetuado com sucesso!");
        }
        else
            Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
    }

    public void Transferir(int numeroOrigem, int numeroDestino, decimal valor)
    {
        Conta? contaOrigem = BuscarNaCollection(numeroOrigem);
        Conta? contaDestino = BuscarNaCollection(numeroDestino);

        if (contaOrigem != null && contaDestino != null)
        {
            // Se o saque na origem der certo, deposita no destino
            if (contaOrigem.Sacar(valor) == true)
            {
                contaDestino.Depositar(valor);
                Console.WriteLine($"\nTransferência de {valor.ToString("C")} efetuada com sucesso!");
            }
        }
        else
        {
            Console.WriteLine("\nConta de Origem e/ou Conta de Destino não encontradas!");
        }
    }

    
}
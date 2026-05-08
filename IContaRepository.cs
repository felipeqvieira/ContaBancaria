using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaBancaria
{
    public interface IContaRepository
    {
        void ProcurarPorNumero(int numero);
        void ListarTodas();
        void Cadastrar(Conta conta);
        void Atualizar(Conta conta);
        void Deletar(int numero);

        void Sacar(int numero, decimal valor);
        void Depositar(int numero, decimal valor);
        void Transferir(int numeroOrigem, int numeroDestino, decimal valor);

        // adicionando na interface para poder retornar a conta na opção de atualizar conta
        Conta? BuscarNaCollection(int numero);
    }
}
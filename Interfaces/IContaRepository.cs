using System;
using ContaBancaria.Models;

namespace ContaBancaria.Interfaces
{
    /// <summary>
    /// Define o contrato para o repositório de contas bancárias.
    /// Especifica as operações fundamentais de persistência e manipulação de dados.
    /// </summary>
    public interface IContaRepository
    {
        /// <summary>
        /// Procura uma conta pelo número e exibe os seus dados no console.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser procurada.</param>
        void ProcurarPorNumero(int numero);

        /// <summary>
        /// Lista todas as contas registadas no repositório.
        /// </summary>
        void ListarTodas();

        /// <summary>
        /// Adiciona uma nova conta ao repositório.
        /// </summary>
        /// <param name="conta">O objeto da conta a ser registado.</param>
        void Cadastrar(Conta conta);

        /// <summary>
        /// Atualiza os dados cadastrais de uma conta existente.
        /// </summary>
        /// <param name="conta">O objeto da conta contendo os dados atualizados.</param>
        void Atualizar(Conta conta);

        /// <summary>
        /// Remove uma conta do repositório com base no seu número.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser removida.</param>
        void Deletar(int numero);

        /// <summary>
        /// Efetua o levantamento (saque) de um valor específico de uma conta.
        /// </summary>
        /// <param name="numero">O número identificador da conta.</param>
        /// <param name="valor">O montante a ser levantado.</param>
        void Sacar(int numero, decimal valor);

        /// <summary>
        /// Efetua o depósito de um valor específico numa conta.
        /// </summary>
        /// <param name="numero">O número identificador da conta.</param>
        /// <param name="valor">O montante a ser depositado.</param>
        void Depositar(int numero, decimal valor);

        /// <summary>
        /// Transfere um valor de uma conta de origem para uma conta de destino.
        /// </summary>
        /// <param name="numeroOrigem">O número identificador da conta remetente.</param>
        /// <param name="numeroDestino">O número identificador da conta beneficiária.</param>
        /// <param name="valor">O montante a ser transferido.</param>
        void Transferir(int numeroOrigem, int numeroDestino, decimal valor);

        /// <summary>
        /// Procura e retorna a referência de uma conta no repositório.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser procurada.</param>
        /// <returns>O objeto <see cref="Conta"/> correspondente, ou null se não for encontrada.</returns>
        Conta? BuscarNaCollection(int numero);
    }
}
using System;
using ContaBancaria.Models;
using ContaBancaria.Controllers;
using ContaBancaria.Utils;

public class Menu
{
    public static void Main(string[] args)
    {
        ContaController contas = new();
        int opcao;

        while (true)
        {
            Console.Clear(); // Limpa a tela a cada nova iteração do menu
            
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(Cores.CorTextoAmarelo + "*****************************************************");
            Console.WriteLine("                                                     ");
            Console.WriteLine("                BANCO ACELERA MAKER                    ");
            Console.WriteLine("                                                     ");
            Console.WriteLine("*****************************************************");
            Console.WriteLine("                                                     ");
            Console.WriteLine("            1 - Criar Conta                          ");
            Console.WriteLine("            2 - Listar Contas                        ");
            Console.WriteLine("            3 - Buscar Conta                         ");
            Console.WriteLine("            4 - Atualizar Dados da Conta             ");
            Console.WriteLine("            5 - Apagar Conta                         ");
            Console.WriteLine("            6 - Sacar                                ");
            Console.WriteLine("            7 - Depositar                            ");
            Console.WriteLine("            8 - Transferir                           ");
            Console.WriteLine("            9 - Sair                                 ");
            Console.WriteLine("                                                     ");
            Console.WriteLine("*****************************************************");
            Console.Write("Entre com a opção desejada: " + Cores.CorReset);

            // verifica conversão para int
            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine(Cores.CorTextoVermelho + "\nPor favor, digite apenas números inteiros." + Cores.CorReset);
                opcao = 0; // opção inválida
            }

            // opção de saída
            if (opcao == 9)
            {
                Console.WriteLine("Obrigado por utilizar nosso sistema!" + Cores.CorReset);
                Environment.Exit(0);
            }

            // opções para gerenciamento das contas
            switch (opcao)
            {
                // criar conta
                case 1:
                    Console.WriteLine(Cores.CorTextoAzul + "\nCriar Conta\n" + Cores.CorReset);

                    // valida agência
                    var (agencia, c1) = InputHelper.LerInteiro(
                        "Digite o Número da Agência (ou '0' para cancelar): ",
                        "Agência inválida! Digite um número inteiro maior que zero.");
                    if (c1) break;

                    // validação de titular
                    var (titular, c2) = InputHelper.LerString(
                        "Digite o Nome do Titular (ou '0' para cancelar): ",
                        "Nome inválido! Digite apenas letras e espaços.",
                        validacao: s => !string.IsNullOrWhiteSpace(s)
                                     && s.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
                    if (c2) break;

                    // validação do tipo
                    var (tipo, c3) = InputHelper.LerInteiro(
                        "Digite o Tipo da Conta (1 - Corrente | 2 - Poupança | 0 - Cancelar): ",
                        "Tipo inválido! Digite 1 ou 2.",
                        validacao: v => v == 1 || v == 2);
                    if (c3) break;

                    decimal saldoInicial = 0m;

                    if (tipo == 1) // Corrente
                    {
                        // Usa cancelToken "-1" pois 0 é um valor válido de limite
                        var (limite, c4) = InputHelper.LerDecimal(
                            "Digite o Limite da Conta (R$) (ou '-1' para cancelar): ",
                            "Limite inválido! Digite um valor numérico positivo.",
                            validacao: v => v >= 0,
                            cancelToken: "-1");
                        if (c4) break;

                        // cria instância da conta corrente
                        contas.Cadastrar(new ContaCorrente(0, agencia, (TipoConta)tipo, titular, saldoInicial, limite));
                    }
                    else // Poupança
                    {
                        var (aniversario, c4) = InputHelper.LerInteiro(
                            "Digite o Dia do Aniversário da Conta (1 a 28) (ou '0' para cancelar): ",
                            "Dia inválido! Digite um número entre 1 e 28.",
                            validacao: v => v >= 1 && v <= 28);
                        if (c4) break;

                        // cria instância da conta poupança
                        contas.Cadastrar(new ContaPoupanca(0, agencia, (TipoConta)tipo, titular, saldoInicial, aniversario));
                    }
                    break;

                // listar todas as contas 
                case 2:
                    Console.WriteLine(Cores.CorTextoAzul + "\nListar todas as Contas\n" + Cores.CorReset);
                    contas.ListarTodas();
                    break;
                    
                // Consultar conta por número
                case 3:
                    Console.WriteLine(Cores.CorTextoAzul + "\nConsultar dados da Conta - por número\n" + Cores.CorReset);

                    var (numeroBusca, cancelouBusca) = InputHelper.LerInteiro(
                        "Digite o número da conta (ou '0' para cancelar): ",
                        "Número da conta inválido! Digite um número inteiro maior que zero.");
                    if (cancelouBusca) break;

                    contas.ProcurarPorNumero(numeroBusca);
                    break;

                // Atualizar dados da conta
                case 4:
                    Console.WriteLine(Cores.CorTextoAzul + "\nAtualizar dados da Conta\n" + Cores.CorReset);

                    var (numeroAtualizar, cancelouAtualizar) = InputHelper.LerInteiro(
                        "Digite o número da conta (ou '0' para cancelar): ",
                        "Número da conta inválido!");
                    if (cancelouAtualizar) break;

                    // Recupera conta atual para referência
                    var contaExistente = contas.BuscarNaCollection(numeroAtualizar);

                    if (contaExistente == null)
                    {
                        Console.WriteLine(Cores.CorTextoVermelho + $"\nA Conta número {numeroAtualizar} não foi encontrada!" + Cores.CorReset);
                        break;
                    }

                    // Atualiza (ou mantém) agência
                    var (novaAgencia, ca1) = InputHelper.LerInteiroOuManter(
                        $"Digite o novo número da Agência (Atual: {contaExistente.Agencia}) [Enter para manter]: ",
                        "Agência inválida! Digite um número inteiro maior que zero.",
                        contaExistente.Agencia);
                    if (ca1) break;

                    // Atualiza (ou mantém) titular
                    var (novoTitular, ca2) = InputHelper.LerStringOuManter(
                        $"Digite o novo nome do Titular (Atual: {contaExistente.Titular}) [Enter para manter]: ",
                        "Nome inválido! Digite apenas letras e espaços.",
                        contaExistente.Titular,
                        validacao: s => !string.IsNullOrWhiteSpace(s)
                                     && s.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
                    if (ca2) break;

                    // Atualiza (ou mantém) tipo
                    var (novoTipo, ca3) = InputHelper.LerInteiroOuManter(
                        $"Digite o novo Tipo (1-Conta Corrente / 2-Conta Poupança) (Atual: {(int)contaExistente.Tipo}) [Enter para manter]: ",
                        "Tipo inválido! Digite 1 ou 2.",
                        (int)contaExistente.Tipo,
                        validacao: v => v == 1 || v == 2);
                    if (ca3) break;

                    // Cria instância com dados atualizados com base no tipo escolhido
                    if (novoTipo == 1)
                    {
                        // Preserva o limite se a conta já for ContaCorrente
                        decimal limiteAtual = contaExistente is ContaCorrente ccExist ? ccExist.Limite : 0m;

                        var (novoLimite, ca4) = InputHelper.LerDecimalOuManter(
                            $"Limite (R$) (Atual: {limiteAtual:C}) [Enter para manter]: ",
                            "Limite inválido! Digite um valor numérico positivo.",
                            limiteAtual,
                            validacao: v => v >= 0);
                        if (ca4) break;

                        contas.Atualizar(new ContaCorrente(numeroAtualizar, novaAgencia, (TipoConta)novoTipo, novoTitular, contaExistente.Saldo, novoLimite));
                    }
                    else
                    {
                        // Preserva o aniversário se a conta já for ContaPoupanca
                        int anivAtual = contaExistente is ContaPoupanca cpExist ? cpExist.Aniversario : 1;

                        var (novoAniv, ca4) = InputHelper.LerInteiroOuManter(
                            $"Aniversário (1-28) (Atual: {anivAtual}) [Enter para manter]: ",
                            "Dia inválido! Digite um número entre 1 e 28.",
                            anivAtual,
                            validacao: v => v >= 1 && v <= 28);
                        if (ca4) break;

                        contas.Atualizar(new ContaPoupanca(numeroAtualizar, novaAgencia, (TipoConta)novoTipo, novoTitular, contaExistente.Saldo, novoAniv));
                    }
                    break;
                
                // apagar conta
                case 5:
                    Console.WriteLine(Cores.CorTextoAzul + "\nApagar a Conta\n" + Cores.CorReset);

                    var (numeroApagar, cancelouApagar) = InputHelper.LerInteiro(
                        "Digite o número da conta a ser apagada (ou '0' para cancelar): ",
                        "Número da conta inválido!");
                    if (cancelouApagar) break;

                    contas.Deletar(numeroApagar);
                    break;

                // saque
                case 6:
                    Console.WriteLine(Cores.CorTextoAzul + "\nSaque\n" + Cores.CorReset);

                    var (numeroSaque, cs1) = InputHelper.LerInteiro(
                        "Digite o número da conta (ou '0' para cancelar): ",
                        "Número da conta inválido! Digite um número inteiro maior que zero.");
                    if (cs1) break;

                    var (valorSaque, cs2) = InputHelper.LerDecimal(
                        "Digite o valor do saque (R$) (ou '0' para cancelar): ",
                        "Valor inválido! Digite um valor numérico maior que zero.");
                    if (cs2) break;

                    contas.Sacar(numeroSaque, valorSaque);
                    break;

                // depósito
                case 7:
                    Console.WriteLine(Cores.CorTextoAzul + "\nDepósito\n" + Cores.CorReset);

                    var (numeroDeposito, cd1) = InputHelper.LerInteiro(
                        "Digite o número da conta (ou '0' para cancelar): ",
                        "Número da conta inválido! Digite um número inteiro maior que zero.");
                    if (cd1) break;

                    var (valorDeposito, cd2) = InputHelper.LerDecimal(
                        "Digite o valor do depósito (R$) (ou '0' para cancelar): ",
                        "Valor inválido! Digite um valor numérico positivo.");
                    if (cd2) break;

                    contas.Depositar(numeroDeposito, valorDeposito);
                    break;

                // transferência
                case 8:
                    Console.WriteLine(Cores.CorTextoAzul + "\nTransferência entre Contas\n" + Cores.CorReset);

                    // validação conta origem
                    var (numeroOrigem, ct1) = InputHelper.LerInteiro(
                        "Digite o número da conta de origem (ou '0' para cancelar): ",
                        "Número da conta inválido! Digite um número inteiro maior que zero.");
                    if (ct1) break;

                    // validação conta destino (não pode ser igual à origem)
                    var (numeroDestino, ct2) = InputHelper.LerInteiro(
                        "Digite o número da conta de destino (ou '0' para cancelar): ",
                        "Número da conta inválido! Digite um número inteiro maior que zero e diferente da conta de origem.",
                        validacao: v => v > 0 && v != numeroOrigem);
                    if (ct2) break;

                    // validação valor de transferência
                    var (valorTransferencia, ct3) = InputHelper.LerDecimal(
                        "Digite o valor da transferência (R$) (ou '0' para cancelar): ",
                        "Valor inválido! Digite um valor numérico positivo.");
                    if (ct3) break;

                    contas.Transferir(numeroOrigem, numeroDestino, valorTransferencia);
                    break;

                // opção inválida
                default:
                    Console.WriteLine(Cores.CorTextoVermelho + "\nOpção Inválida!\n" + Cores.CorReset);
                    break;
            }

            // Pausa exibição do menu para não poluir a tela
            Console.WriteLine(Cores.CorTextoAmarelo + "\nPressione Enter para continuar..." + Cores.CorReset);
            Console.ReadLine();
        }
    }
}
using System;
using ContaBancaria;

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
            Console.WriteLine("            8 - Transferir valores entre Contas      ");
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
                    
                    bool cancelar = false; // flag para cancelar operação

                    // valida agência
                    int agencia = 0;
                    while (true)
                    {
                        Console.Write("Digite o Número da Agência (ou '0' para cancelar): ");

                        // verifica se é vazia
                        string inputAgencia = Console.ReadLine() ?? string.Empty;

                        // se for zero, sai
                        if (inputAgencia == "0") { cancelar = true; break; }

                        //verifica se é inteiro e maior que zero
                        if (int.TryParse(inputAgencia, out agencia) && agencia > 0) break;
                        
                        Console.WriteLine(Cores.CorTextoVermelho + "Agência inválida! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelar) break;

                    // validação de titular
                    string titular = string.Empty;
                    while (true)
                    {
                        Console.Write("Digite o Nome do Titular (ou '0' para cancelar): ");
                        // verifica se é vazia
                        titular = Console.ReadLine() ?? string.Empty;

                        //se for zero, sai
                        if (titular == "0") { cancelar = true; break; }

                        // valida se for vazia ou espaço em branco e tiver só caractere
                        if (!string.IsNullOrWhiteSpace(titular) && titular.All(c => char.IsLetter(c) || char.IsWhiteSpace(c))) break;
                        
                        Console.WriteLine(Cores.CorTextoVermelho + "Nome inválido! Digite apenas letras e espaços." + Cores.CorReset + "\n");
                    }
                    if (cancelar) break;

                    // validação do tipo
                    int tipo = 0;
                    while (true)
                    {
                        Console.Write("Digite o Tipo da Conta (1 - Corrente | 2 - Poupança | 0 - Cancelar): ");
                        // verifica se é vazia
                        string inputTipo = Console.ReadLine() ?? string.Empty;

                        // se for zero, sai
                        if (inputTipo == "0") { cancelar = true; break; }

                        //verifica se é inteiro e se é 1 ou 2
                        if (int.TryParse(inputTipo, out tipo) && (tipo == 1 || tipo == 2)) break;
                        
                        Console.WriteLine(Cores.CorTextoVermelho + "Tipo inválido! Digite 1 ou 2." + Cores.CorReset + "\n");
                    }
                    if (cancelar) break;

                    decimal saldoInicial = 0m;
                    
                    if (tipo == 1) // Corrente
                    {
                        decimal limite = 0m;
                        while (true)
                        {
                            Console.Write("Digite o Limite da Conta (R$) (ou '-1' para cancelar): ");
                            // verifica se limite é vazio
                            string inputLimite = Console.ReadLine() ?? string.Empty;
                            // se for -1 cancela
                            if (inputLimite == "-1") { cancelar = true; break; }
                            //verifica se é maior ou igual a zero e se é inteiro
                            if (decimal.TryParse(inputLimite, out limite) && limite >= 0) break;
                            
                            Console.WriteLine(Cores.CorTextoVermelho + "Limite inválido! Digite um valor numérico positivo." + Cores.CorReset + "\n");
                        }
                        if (cancelar) break;

                        // cria instância da conta com dados atualizados
                        ContaCorrente cc = new ContaCorrente(0, agencia, (TipoConta)tipo, titular, saldoInicial, limite);
                        // cadastra conta
                        contas.Cadastrar(cc);
                    }
                    else if (tipo == 2) // Poupança
                    {
                        int aniversario = 0;
                        while (true)
                        {
                            Console.Write("Digite o Dia do Aniversário da Conta (1 a 28) (ou '0' para cancelar): ");
                            //verifica se é vazio
                            string inputAniv = Console.ReadLine() ?? string.Empty;

                            // se for zero, sai
                            if (inputAniv == "0") { cancelar = true; break; }

                            // verifica se é inteiro e se está entre 1 e 28
                            if (int.TryParse(inputAniv, out aniversario) && aniversario >= 1 && aniversario <= 28) break;
                            
                            Console.WriteLine(Cores.CorTextoVermelho + "Dia inválido! Digite um número entre 1 e 28." + Cores.CorReset + "\n");
                        }
                        if (cancelar) break;

                        // cria instância com dados atualizados
                        ContaPoupanca cp = new ContaPoupanca(0, agencia, (TipoConta)tipo, titular, saldoInicial, aniversario);
                        //cadastra conta
                        contas.Cadastrar(cp);
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

                    int numeroBusca = 0;
                    
                    while (true)
                    {
                        Console.Write("Digite o número da conta (ou '0' para cancelar): ");
                        string inputNumero = Console.ReadLine() ?? string.Empty;

                        //se digitar 0, sai do laço while e encerra o case
                        if (inputNumero == "0") { break; } 

                        // Valida se é um número inteiro positivo
                        if (int.TryParse(inputNumero, out numeroBusca) && numeroBusca > 0)
                        {
                            contas.ProcurarPorNumero(numeroBusca);
                            break; 
                        }
                        
                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    break;

                //Atualizar dados da conta (implementar para ver qual dado ele quer atualizar)
                case 4:
                    Console.WriteLine(Cores.CorTextoAzul + "\nAtualizar dados da Conta\n" + Cores.CorReset);

                    Console.Write("Digite o número da conta: ");
                    if (int.TryParse(Console.ReadLine(), out int numero))
                    {
                        // Recupera conta atual para referência
                        var contaExistente = contas.BuscarNaCollection(numero);

                        if (contaExistente != null)
                        {
                            // Atualiza (ou mantém) agência
                            Console.Write($"Digite o novo número da Agência (Atual: {contaExistente.Agencia}) [Enter para manter]: ");
                            
                            string inputAgencia = Console.ReadLine() ?? "";
                            
                            int novaAgencia = string.IsNullOrWhiteSpace(inputAgencia) ? contaExistente.Agencia : Convert.ToInt32(inputAgencia);

                            // Atualiza (ou mantém) titular
                            Console.Write($"Digite o novo nome do Titular (Atual: {contaExistente.Titular}) [Enter para manter]: ");
                            
                            string inputTitular = Console.ReadLine() ?? "";
                            
                            string novoTitular = string.IsNullOrWhiteSpace(inputTitular) ? contaExistente.Titular : inputTitular;

                            // Atualiza (ou mantém) tipo
                            Console.Write($"Digite o novo Tipo (1-Conta Corrente / 2-Conta Poupança) (Atual: {(int)contaExistente.Tipo}) [Enter para manter]: ");
                            
                            string inputTipo = Console.ReadLine() ?? "";
                            
                            int novoTipo = string.IsNullOrWhiteSpace(inputTipo) ? (int)contaExistente.Tipo : Convert.ToInt32(inputTipo);

                            // Cria instância com dados atualizados e/ou mantidos
                            // Talvez criar algo aqui que se nenhum dado for atualizado, então não precisa criar instância
                            if (novoTipo == 1)
                            {
                                var cc = new ContaCorrente(numero, novaAgencia, (TipoConta)novoTipo, novoTitular, contaExistente.Saldo, 0m);
                                // Se for CC, poderíamos pedir o limite também seguindo a mesma lógica
                                contas.Atualizar(cc);
                            }
                            else
                            {
                                var cp = new ContaPoupanca(numero, novaAgencia, (TipoConta)novoTipo, novoTitular, contaExistente.Saldo, 0);
                                contas.Atualizar(cp);
                            }
                        }
                        else
                        {
                            Console.WriteLine(Cores.CorTextoVermelho + $"\nA Conta número {numero} não foi encontrada!" + Cores.CorReset);
                        }
                    }
                    break;
                
                //apagar conta
                case 5:
                    Console.WriteLine(Cores.CorTextoAzul + "\nApagar a Conta\n" + Cores.CorReset);

                    int numeroApagar = 0;
                    while (true)
                    {
                        Console.Write("Digite o número da conta a ser apagada (ou '0' para cancelar): ");
                        string inputNumero = Console.ReadLine() ?? string.Empty;

                        if (inputNumero == "0") break; // Sai para o menu

                        if (int.TryParse(inputNumero, out numeroApagar) && numeroApagar > 0)
                        {
                            contas.Deletar(numeroApagar);
                            break; 
                        }
                        
                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido!" + Cores.CorReset + "\n");
                    }
                    break;

                // saque
                case 6:
                    Console.WriteLine(Cores.CorTextoAzul + "\nSaque\n" + Cores.CorReset);

                    int numeroSaque = 0;
                    bool cancelarSaque = false;

                    // Valida número da conta
                    while (true)
                    {
                        Console.Write("Digite o número da conta (ou '0' para cancelar): ");
                        string inputNumero = Console.ReadLine() ?? string.Empty;

                        // se for zero, sai
                        if (inputNumero == "0") { cancelarSaque = true; break; }

                        // inteiro e maior que zero
                        if (int.TryParse(inputNumero, out numeroSaque) && numeroSaque > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelarSaque) break;

                    // validação saque
                    decimal valorSaque = 0m;
                    while (true)
                    {
                        Console.Write("Digite o valor do saque (R$) (ou '0' para cancelar): ");
                        string inputValor = Console.ReadLine() ?? string.Empty;
                        
                        // se for zero, sai
                        if (inputValor == "0") { cancelarSaque = true; break; }

                        //inteiro e maior que zero
                        if (decimal.TryParse(inputValor, out valorSaque) && valorSaque > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Valor inválido! Digite um valor numérico maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelarSaque) break;

                    // realiza saque
                    contas.Sacar(numeroSaque, valorSaque);
                    
                    break;

                // depósito
                case 7:
                    Console.WriteLine(Cores.CorTextoAzul + "\nDepósito\n" + Cores.CorReset);

                    int numeroDeposito = 0;
                    bool cancelarDeposito = false;

                    // Valida número da conta
                    while (true)
                    {
                        Console.Write("Digite o número da conta (ou '0' para cancelar): ");
                        string inputNumero = Console.ReadLine() ?? string.Empty;

                        // se for zero sai
                        if (inputNumero == "0") { cancelarDeposito = true; break; }

                        // inteiro e maior que zero
                        if (int.TryParse(inputNumero, out numeroDeposito) && numeroDeposito > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelarDeposito) break;

                    decimal valorDeposito = 0m;

                    //validação do depósito
                    while (true)
                    {
                        Console.Write("Digite o valor do depósito (R$) (ou '0' para cancelar): ");
                        string inputValor = Console.ReadLine() ?? string.Empty;
                        
                        // se for zero sai
                        if (inputValor == "0") { cancelarDeposito = true; break; }

                        // inteiro e maior que zero
                        if (decimal.TryParse(inputValor, out valorDeposito) && valorDeposito > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Valor inválido! Digite um valor numérico positivo." + Cores.CorReset + "\n");
                    }
                    if (cancelarDeposito) break;

                    // chama depositar
                    contas.Depositar(numeroDeposito, valorDeposito);
                    
                    break;

                // transferência
                case 8:
                    Console.WriteLine(Cores.CorTextoAzul + "\nTransferência entre Contas\n" + Cores.CorReset);

                    int numeroOrigem = 0;
                    bool cancelarTransferencia = false;

                    // validação conta origem
                    while (true)
                    {
                        Console.Write("Digite o número da conta de origem (ou '0' para cancelar): ");
                        string inputOrigem = Console.ReadLine() ?? string.Empty;
                        
                        // se for zero sai
                        if (inputOrigem == "0") { cancelarTransferencia = true; break; }

                        // inteiro e maior que zero
                        if (int.TryParse(inputOrigem, out numeroOrigem) && numeroOrigem > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelarTransferencia) break;

                    // validação conta destino
                    int numeroDestino = 0;
                    while (true)
                    {
                        Console.Write("Digite o número da conta de destino (ou '0' para cancelar): ");
                        string inputDestino = Console.ReadLine() ?? string.Empty;

                        // se for zero sai
                        if (inputDestino == "0") { cancelarTransferencia = true; break; }

                        // inteiro e maior que zero
                        if (int.TryParse(inputDestino, out numeroDestino) && numeroDestino > 0)
                        {
                            // Validação extra: Destino não pode ser igual a Origem
                            if (numeroDestino != numeroOrigem)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine(Cores.CorTextoVermelho + "A conta de destino não pode ser a mesma de origem!" + Cores.CorReset + "\n");
                                continue; // Reinicia o laço while
                            }
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Número da conta inválido! Digite um número inteiro maior que zero." + Cores.CorReset + "\n");
                    }
                    if (cancelarTransferencia) break;

                    // validaçaõ valor de transferência
                    decimal valorTransferencia = 0m;
                    while (true)
                    {
                        Console.Write("Digite o valor da transferência (R$) (ou '0' para cancelar): ");
                        string inputValor = Console.ReadLine() ?? string.Empty;

                        // se for zero sai
                        if (inputValor == "0") { cancelarTransferencia = true; break; }

                        // inteiro e maior que zero
                        if (decimal.TryParse(inputValor, out valorTransferencia) && valorTransferencia > 0)
                        {
                            break;
                        }

                        Console.WriteLine(Cores.CorTextoVermelho + "Valor inválido! Digite um valor numérico positivo." + Cores.CorReset + "\n");
                    }
                    if (cancelarTransferencia) break;

                    // chama transferir
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
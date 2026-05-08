using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ContaBancaria.Models;    
using ContaBancaria.Interfaces; 
using ContaBancaria.Utils;

namespace ContaBancaria.Controllers
{

    /// <summary>
    /// Classe responsável pela gestão da coleção de contas bancárias.
    /// Implementa as operações definidas em <see cref="IContaRepository"/> e gere a persistência dos dados em ficheiro JSON.
    /// </summary>
    public class ContaController : IContaRepository
    {
        private List<Conta> listaContas = new List<Conta>();
        private int numeroId = 0;

        /// <summary>
        /// Caminho absoluto para o ficheiro de persistência de dados.
        /// Utiliza o diretório base da aplicação para evitar problemas com caminhos relativos.
        /// </summary>
        private readonly string _caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "contas.json");

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ContaController"/> e carrega os dados previamente guardados.
        /// </summary>
        public ContaController()
        {
            CarregarDoArquivo();
        }

        /// <summary>
        /// Guarda o estado atual da lista de contas num ficheiro JSON.
        /// Utiliza escrita atômica através de um arquivo temporário para prevenir a corrupção de dados em caso de falha.
        /// </summary>
        private void SalvarNoArquivo()
        {
            string caminhoTemp = _caminhoArquivo + ".tmp";

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(listaContas, options);
                
                File.WriteAllText(caminhoTemp, jsonString);
                File.Move(caminhoTemp, _caminhoArquivo, true);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(Cores.CorTextoVermelho + "\n[Erro] O sistema não possui permissão de escrita na pasta atual." + Cores.CorReset);
            }
            catch (IOException ex)
            {
                Console.WriteLine(Cores.CorTextoVermelho + $"\n[Erro de E/S] Não foi possível aceder ao disco: {ex.Message}" + Cores.CorReset);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Cores.CorTextoVermelho + $"\n[Erro Crítico] Falha ao guardar os dados: {ex.Message}" + Cores.CorReset);
            }
            finally
            {
                if (File.Exists(caminhoTemp))
                {
                    try { File.Delete(caminhoTemp); } catch { /* Ignora erro na limpeza de segurança */ }
                }
            }
        }

        /// <summary>
        /// Carrega os dados do ficheiro JSON para a memória.
        /// Cria backups automaticamente caso o ficheiro original esteja corrompido.
        /// </summary>
        private void CarregarDoArquivo()
        {
            //verifica se o arquivo existe
            if (!File.Exists(_caminhoArquivo)) return;

            try
            {
                // le o arquivo
                string jsonString = File.ReadAllText(_caminhoArquivo);
                // carrega as contas
                var contasCarregadas = JsonSerializer.Deserialize<List<Conta>>(jsonString);
                
                // se tiver contas
                if (contasCarregadas != null)
                {
                    //coloca na lista
                    listaContas = contasCarregadas;
                    if (listaContas.Count > 0)
                    {
                        numeroId = listaContas.Max(c => c.Numero);
                    }
                }
            }
            // se der erro Json, cria arquivo backup
            catch (JsonException)
            {
                string arquivoBackup = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"contas_backup_{DateTime.Now:yyyyMMddHHmmss}.json");
                File.Move(_caminhoArquivo, arquivoBackup);
                
                Console.WriteLine(Cores.CorTextoVermelho + $"\n[Aviso] O ficheiro de dados estava corrompido e não pôde ser lido." + Cores.CorReset);
                Console.WriteLine(Cores.CorTextoAmarelo + $"Um backup foi criado em: {arquivoBackup}" + Cores.CorReset);
                Console.WriteLine(Cores.CorTextoAmarelo + "A aplicação será iniciada com o banco de dados limpo.\n" + Cores.CorReset);
                
                listaContas = new List<Conta>();
            }
            // outros erros
            catch (Exception ex)
            {
                Console.WriteLine(Cores.CorTextoVermelho + $"\n[Erro do Sistema] Falha ao carregar os dados: {ex.Message}" + Cores.CorReset);
            }
        }

        /// <summary>
        /// Gera de forma incremental um novo número identificador único para as contas.
        /// </summary>
        /// <returns>Um número inteiro único correspondente ao próximo ID.</returns>
        private int GerarNumero()
        {
            numeroId++;
            return numeroId;
        }

        /// <summary>
        /// Procura e retorna a referência de uma conta na coleção em memória.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser procurada.</param>
        /// <returns>O objeto <see cref="Conta"/> correspondente, ou null se não for encontrada.</returns>
        public Conta? BuscarNaCollection(int numero)
        {
            foreach (var conta in listaContas)
            {
                if (conta.Numero == numero)
                    return conta;
            }
            return null;
        }

        /// <summary>
        /// Adiciona uma nova conta ao repositório e executa a persistência dos dados no arquivo.
        /// </summary>
        /// <param name="conta">O objeto da conta a ser cadastrado.</param>
        /// <exception cref="ArgumentNullException">Lançada quando a conta informada é nula.</exception>
        public void Cadastrar(Conta conta)
        {
            if (conta == null)
            {
                throw new ArgumentNullException(nameof(conta), "Não é possível registar uma conta nula.");
            }

            conta.Numero = GerarNumero();
            listaContas.Add(conta);

            SalvarNoArquivo();
            Console.WriteLine($"\nA Conta número {conta.Numero} foi criada com sucesso!");
        }

        /// <summary>
        /// Lista as contas e seus dados correspondentes
        /// </summary>
        public void ListarTodas()
        {
            if (listaContas.Count == 0)
            {
                Console.WriteLine("\nNão existem contas registadas no sistema.");
                return;
            }

            foreach (var conta in listaContas)
            {
                conta.Visualizar();
            }
        }

        /// <summary>
        /// Procura uma conta pelo número e exibe os seus detalhes formataods no console.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser procurada.</param>
        public void ProcurarPorNumero(int numero)
        {
            Conta? conta = BuscarNaCollection(numero);

            if (conta != null)
                conta.Visualizar();
            else
                Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
        }

        /// <summary>
        /// Substitui os dados de uma conta existente na coleção e atualiza o arquivo de persistência.
        /// </summary>
        /// <param name="contaAtualizada">O objeto da conta contendo os dados modificados.</param>
        public void Atualizar(Conta contaAtualizada)
        {
            int index = listaContas.FindIndex(c => c.Numero == contaAtualizada.Numero);

            if (index != -1)
            {
                listaContas[index] = contaAtualizada;
                SalvarNoArquivo();
                Console.WriteLine($"\nA Conta número {contaAtualizada.Numero} foi atualizada com sucesso!");
            }
            else
            {
                Console.WriteLine($"\nA Conta número {contaAtualizada.Numero} não foi encontrada!");
            }
        }

        /// <summary>
        /// Remove uma conta da coleção com base no seu número e reflete a exclusão no arquivo de dados.
        /// </summary>
        /// <param name="numero">O número identificador da conta a ser removida.</param>
        public void Deletar(int numero)
        {
            Conta? contaBuscada = BuscarNaCollection(numero);

            if (contaBuscada != null)
            {
                if(listaContas.Remove(contaBuscada))
                {
                    SalvarNoArquivo();
                    Console.WriteLine($"\nA Conta número {numero} foi eliminada com sucesso!");
                }
            }
            else
            {
                Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
            }
        }

        /// <summary>
        /// Efetua o depósito de um montante financeiro numa conta específica e salva o novo estado.
        /// </summary>
        /// <param name="numero">O número identificador da conta.</param>
        /// <param name="valor">O montante a ser depositado.</param>
        public void Depositar(int numero, decimal valor)
        {
            Conta? contaBuscada = BuscarNaCollection(numero);

            if (contaBuscada != null)
            {
                contaBuscada.Depositar(valor);
                SalvarNoArquivo();
                Console.WriteLine($"\nDepósito de {valor:C} efetuado com sucesso!");
            }
            else
            {
                Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
            }
        }

        /// <summary>
        /// Efetua o saque de um valor de uma conta, validando a disponibilidade de saldo, e persiste a alteração.
        /// </summary>
        /// <param name="numero">O número identificador da conta.</param>
        /// <param name="valor">O montante a ser sacado.</param>
        public void Sacar(int numero, decimal valor)
        {
            Conta? contaBuscada = BuscarNaCollection(numero);

            if (contaBuscada != null)
            {
                if (contaBuscada.Sacar(valor))
                {
                    SalvarNoArquivo();
                    Console.WriteLine($"\nLevantamento de {valor:C} efetuado com sucesso!");
                }
            }
            else
            {
                Console.WriteLine($"\nA Conta número {numero} não foi encontrada!");
            }
        }

        /// <summary>
        /// Realiza a transferência de valores entre duas contas, validando regras de negócio e atualizando o repositório geral.
        /// </summary>
        /// <param name="numeroOrigem">O número identificador da conta remetente.</param>
        /// <param name="numeroDestino">O número identificador da conta beneficiária.</param>
        /// <param name="valor">O montante a ser transferido.</param>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor da transferência é menor ou igual a zero.</exception>
        public void Transferir(int numeroOrigem, int numeroDestino, decimal valor)
        {
            if(valor <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(valor), "O valor da transferência deve ser maior que zero.");
            }

            Conta? contaOrigem = BuscarNaCollection(numeroOrigem);
            Conta? contaDestino = BuscarNaCollection(numeroDestino);

            if (contaOrigem != null && contaDestino != null)
            {
                if (contaOrigem.Sacar(valor))
                {
                    contaDestino.Depositar(valor);
                    SalvarNoArquivo();                                                                                          
                    Console.WriteLine($"\nTransferência de {valor:C} efetuada com sucesso!");
                }
            }
            else
            {
                Console.WriteLine("\nConta de Origem e/ou Conta de Destino não encontradas!");
            }
        }
    }
}
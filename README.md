# Banco Acelera Maker - Sistema Bancário em C#

## Síntese do Projeto
---
O Banco Acelera Maker é uma aplicação de console interativa desenvolvida em C# (.NET 10). O projeto simula o ecossistema de gerenciamento de contas bancárias de uma instituição financeira, suportando o controle completo e a persistência de dados em arquivos locais.
A aplicação é desenhada para emular operações financeiras do mundo real, gerenciando diferentes modalidades de contas (Conta Corrente e Conta Poupança) por meio de um menu interativo de terminal.

## Funcionalidades
---
O sistema apresenta o seguinte conjunto de funcionalidades, cujo comportamento esperado baseia-se nas regras de negócio implementadas:

### Gestão de Contas
---
1. Criação de novas contas com especialização estrutural (Conta Corrente com Limite e Conta Poupança com Aniversário);
2. Listagem geral e busca específica de contas por número identificador;
3. Atualização de dados: permite a edição parcial, onde o usuário pode pressional Enter para manter o valor original de um campo;
4. Remoção de contas do repositório.

### Operações Financeiros
---
1. Saque: Valida limites de saldo e teto de crédito (Conta corrente) antes de autorizar a operação;
2. Depósito: Adiciona fundos e aplica automaticamente regras de negócio (como rendimentos de juros em dias de aniversário para a Conta Poupança);
3. Transferência: Movimenta valores entre contas distintas, aplicando validações lógicas e de segurança nas duas pontas da transação.

### Experiência do Usuário (UX) e Segurança da Interface:
---
1. Menu interativo no terminal utilizando formatação de cores para alertas e informações;
2. Validação contínua de entradas (uso de Tryparse e loops While), retendo o usuário de forma amigável em caso de erro de digitação sem causar o travamento da aplicação;
3. Rotas de cancelamento durante as operações, permitindo ao usuário abortar fluxos de inserção de dados a qualquer momento.

### Persistência de Dados (Local):
---
1. Serialização polimérfica para gravar e recuperar os diferentes tipos de contas em um único arquivo .json
2. Escrita atômica em disco, minimizando o risco de corrupção do arquivo de banco de dados;
3. Geração automática de backups em caso de detecção de corrupção no arquivo original

## Tecnologias
---
- Linguagem: C#
- Framework: .NET 10.0
- Biblitecas nativas:
  - `System.Text.Json`: para serialização/desserialização de dados;
  - `System.IO`: para operações de leitura e gravação no sistema de arquivos;
  - `System.Linq`: para consultas em coleções.

## Boas práticas
---
Uso extensivo de Herança e Polimorfismo. A classe base abstrata `Conta` delega comportamentos específicos para as suas subclasses (`ContaCorrente` e `ContaPoupanca`).

Validações contínuas de input utilizando métodos `TryParse`, retendo o usuário em malhas de repetição estruturadas (`InputHelper.cs`) em caso de formatações inválidas.

Utilização de Guard Clauses (ex: `throw new ArgumentOutOfRangeException()`) nas classes de modelo e no controlador, abortando operações caso sejam detectados valores nocivos (como saques negativos) ou objetos nulos.

Uso dos atributos `[JsonDerivedType]` orienta o serializador a preservar a identidade das subclasses no arquvio JSON.

O processo de salvamento utiliza arquivos temporários e substituição (`File.Move`).

Todas as classes, métodos e interfaces públicas estão documentadas seguindo o padrão oficial da Microsoft (`/// <summary>`).



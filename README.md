# ContaBancaria
Projeto que visa desenvolver um sistema de contas bancárias em C#.

Utilizei TryParse ao invés de blocos try catch no menu porque o lançamento de exceções é custoso e interrompe o fluxo do usuário.

Troquei as variáveis e retornos de alguns métodos de float para decimal pela precisão monetária e exibição em R$ para o usuário.

Para cada opção no menu, há uma opção de saída para retornar ao menu novamente e cancelar a operação em andamento.

Atualizar Dados Conta permite que o usuário atualize apenas um subconjunto dos dados ao invés de todos eles.
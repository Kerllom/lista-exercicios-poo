# Lista de Exercícios — Programação Orientada a Objetos (SENAI)

Resolução da lista de exercícios da disciplina **Algoritmos e Programação Orientada a Objetos**.

Trabalho desenvolvido em trio. A branch `main` contém os exercícios 1 a 4 em **C#** (parte individual de Kerllom) e o exercício 5 em **Java** (parte colaborativa). Cada integrante mantém seus três primeiros exercícios em sua própria branch, para avaliação individual.

## Tecnologias

- **Linguagem:** C# (.NET) — exercícios 1 a 4
- **Linguagem:** Java — exercício 5
- **Banco de dados:** MySQL
- **Driver C#:** MySql.Data (Connector/NET)

## Organização das branches

| Branch | Conteúdo |
|--------|----------|
| `main` | Exercícios 1–4 (C#) + Exercício 5 (Java, colaborativo) |
| `kerllom` | Os 3 primeiros exercícios de Kerllom |
| `ricardo` | Os 3 primeiros exercícios de Ricardo |
| `mateus` | Os 3 primeiros exercícios de Mateus |

## Estrutura do repositório

Cada exercício é um projeto independente, organizado em camadas:
ExercicioXX-Nome/
├── Models/        # Classes de domínio (encapsulamento)
├── Data/          # Conexao (conexão isolada) + DAOs (acesso a dados)
├── Program.cs     # Menu de console (apresentação)
└── *.csproj

## Exercícios

### Exercício 1 — Cadastro de Clientes

CRUD completo de clientes. Aplica encapsulamento na classe `Cliente`, conexão isolada e unicidade de CPF tratada no banco (`UNIQUE`) e no código.

### Exercício 2 — Controle de Estoque

Relacionamento muitos-para-um entre `Produto` e `Categoria`, com chave estrangeira e listagem por categoria via `JOIN`.

### Exercício 3 — Sistema de Agendamento

Três entidades (`Paciente`, `Medico`, `Consulta`), com a consulta carregando duas chaves estrangeiras. Listagens com `JOIN` de múltiplas tabelas e cancelamento por mudança de status.

### Exercício 4 — Sistema Bancário

`Correntista`, `ContaBancaria` e tabela `extrato`. Depósito e saque usam **transações** (commit/rollback) para garantir atomicidade entre a atualização do saldo e o registro no extrato. Bloqueia saque com saldo insuficiente.

### Exercício 5 — Avaliação de Funcionários *(em Java)*

`Departamento`, `Funcionario` e `Avaliacao`, com cálculo de média e ranking por departamento.

## Como executar (exercícios em C#)

1. Crie o banco rodando o script SQL do exercício no MySQL.
2. Ajuste a string de conexão em `Data/Conexao.cs` (usuário e senha do seu MySQL).
3. No terminal, dentro da pasta do exercício:

```bash
   dotnet restore
   dotnet run
```

## Bancos por exercício

| Exercício | Banco |
|-----------|-------|
| 1 | `loja_clientes` |
| 2 | `estoque` |
| 3 | `clinica` |
| 4 | `banco` |

## Integrantes

- Kerllom
- Ricardo
- Mateus

## Decisões de arquitetura

- **Encapsulamento:** atributos privados expostos por propriedades com validação.
- **Conexão isolada:** cada projeto tem uma classe `Conexao` dedicada.
- **Separação de responsabilidades:** Models (domínio) / Data (DAOs) / Program (apresentação).
- **Persistência real:** todos os dados são gravados no MySQL, nada apenas em memória.
- **Tratamento de erros:** falhas de conexão e operação são exibidas ao usuário com mensagens claras.

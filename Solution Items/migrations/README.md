# Migration Scripts

## Como executar os scripts

Execute sempre a partir da pasta:

``` powershell
Solution Items/migrations/
```

Os comandos a seguir são executados no terminal **PowerShell**.

---

## `add-migration.ps1`

**Como usar:**

``` powershell
.\add-migration.ps1 -MigrationName "NomeDaMigration"
```

**O que faz:**  

- Adiciona uma nova migration ao projeto com o nome especificado.

---

## `update-database.ps1`

**Como usar:**

``` powershell
.\update-database.ps1
```

**O que faz:**  

- Aplica todas as migrations pendentes no banco de dados configurado.

---

## `script-migration.ps1`

**Como usar:**

``` powershell
.\script-migration.ps1
```

**O que faz:**  

- Gera um script SQL idempotente (`script.sql`) com todas as migrations pendentes, ideal para uso em ambientes de produção ou CI/CD.

---

## `list-migrations.ps1`

**Como usar:**

``` powershell
.\list-migrations.ps1
```

**O que faz:**  

- Lista todas as migrations existentes no projeto, na ordem em que foram criadas.

---

## `remove-migration.ps1`

**Como usar:**

``` powershell
.\remove-migration.ps1
```

**O que faz:**  

- Remove a última migration criada no projeto, revertendo as alterações apenas no código.  
- Não desfaz automaticamente alterações que foram aplicadas ao banco com `dotnet ef database update`.  
- Se a migration já foi aplicada, você precisa também desfazer a aplicação no banco com:  

``` powershell
dotnet ef database update <MigrationAnterior>
```

---

## Entendendo a remoção de uma migration no Entity Framework Core

### O que significa “remover uma migration”?

**1. Quando você executa:**

``` powershell
dotnet ef migrations add MinhaMigration
```

- O EF Core cria um arquivo de migration no código (`.cs`) com as instruções de alteração no banco de dados.

**2. Quando você executa:**

``` powershell
dotnet ef database update
```

- O EF Core executa a migration no banco de dados, aplicando fisicamente as alterações (como `CREATE TABLE`, `ALTER COLUMN`, etc.).

**3. Se depois disso você executa:**

``` powershell
dotnet ef migrations remove
```

- O EF remove o arquivo de migration do código, mas não desfaz as alterações já aplicadas no banco.

---

## Por que precisa desfazer manualmente?

Se a migration já foi aplicada ao banco com:

``` powershell
dotnet ef database update
```

Então o banco de dados está num estado alterado.

**Exemplo:**

- Criou a migration: `CreateUserTable`.  
- Aplicou: `dotnet ef database update` → Tabela `Users` criada no banco.  
- Removeu a migration com: `dotnet ef migrations remove`.

**Resultado:**

- No código: a migration foi removida.  
- No banco: a tabela `Users` ainda existe!

---

## Como desfazer corretamente?

1. Use o comando:

``` powershell
dotnet ef database update <MigrationAnterior>
```

O EF irá:

- Verificar as migrations aplicadas no banco (`__EFMigrationsHistory`).
- Executar o método `Down()` da migration, desfazendo as alterações.

---

## Fluxo correto para desfazer uma migration aplicada

**1. Desfazer a aplicação no banco:**

``` powershell
dotnet ef database update NomeDaMigrationAnterior
```

**2. Remover a migration do código:**

``` powershell
dotnet ef migrations remove
```

---

## Exemplo prático

Migrations aplicadas:

``` powershell
20240510_InitialSetup
20240520_CreateUserTable
```

Para remover `CreateUserTable`:

**1. Desfazer no banco:**

``` powershell
dotnet ef database update InitialSetup
```

**2. Remover do código:**

``` powershell
dotnet ef migrations remove
```

---

## O que acontece se não desfazer no banco?

- O banco continuará com a alteração aplicada.
- O código não terá mais a migration.

Isso gera inconsistência e potenciais erros na aplicação ou em novos deploys.

---

## Resumo

| Situação                | Ação                                                   |
|------------------------|--------------------------------------------------------|
| Migration aplicada     | Desfazer com `dotnet ef database update <Anterior>`    |
| Migration não aplicada | Apenas `dotnet ef migrations remove` basta             |

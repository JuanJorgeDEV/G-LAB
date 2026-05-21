# ProjetoOS

Aplicacao ASP.NET Core MVC para registrar problemas em equipamentos, abrir ordens de servico, designar responsaveis e controlar tempo real de execucao.

## Como executar

```powershell
dotnet restore
dotnet run --urls http://localhost:5086
```

Acesse `http://localhost:5086`.

## Usuarios de teste

- Administrador: `admin@projeto.local` / `123`
- Colaborador: `joao@projeto.local` / `123`
- Colaborador: `maria@projeto.local` / `123`

## Banco de dados

O projeto usa PostgreSQL via EF Core/Npgsql quando a connection string estiver configurada em `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true"
}
```

Se a string estiver vazia, o sistema usa banco em memoria para testes locais.

## Funcionalidades do MVP

- Login simples por sessao
- Perfis Colaborador e Administrador
- Cadastro, edicao, listagem e busca de equipamentos
- Abertura de OS com equipamento por NI ou sem NI
- Modal para decidir se o solicitante executa ou envia para OPP
- Painel de ordens e designacao de responsavel
- Execucao com comecar, aguardar, continuar e finalizar
- Controle de tempo trabalhado sem contar tempo em aguardando
- Relatorio basico com filtros e total de horas

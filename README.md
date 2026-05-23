# LifeOS.API

Backend do LifeOS, um sistema de gerenciamento de habitos e tarefas com integracao futura com Google Calendar e um aplicativo mobile consumindo a API.

## Stack

- ASP.NET Core
- PostgreSQL
- Entity Framework Core
- JWT
- Refresh Token
- FluentValidation
- MediatR

## Estrutura da solucao

O backend foi organizado em camadas dentro de `src`:

- `Api`
- `Application`
- `Domain`
- `Infra.Data`
- `Infra.IoC`

### Organizacao por feature

A camada `Application` deve ser dividida por feature:

- `Application/Tasks`
- `Application/Habits`
- `Application/Planner`
- `Application/Auth`
- `Application/Calendar`

Cada feature deve concentrar seus comandos, consultas, handlers, DTOs e validacoes.

## Primeiro passo do produto

A primeira entrega funcional do backend deve ser a autenticacao:

1. Cadastro de usuario.
2. Login.
3. Geracao de JWT.
4. Refresh token.

Depois disso, o projeto deve evoluir para tarefas, habitos, planner e calendario.

## Regras de arquitetura

- `Domain` deve conter apenas regras de negocio puras.
- `Application` deve conter os casos de uso.
- `Infra.Data` deve tratar persistencia com EF Core e PostgreSQL.
- `Infra.IoC` deve centralizar a injeccao de dependencias.
- `Api` deve expor os endpoints HTTP e a configuracao da aplicacao.

## Como executar

```bash
dotnet restore
dotnet build LifeOS.API.sln
dotnet run --project src/Api/LifeOS.Api.csproj
```

## Roadmap

- Auth: cadastro, login, JWT e refresh token.
- Tasks: criacao, listagem, conclusao e organizacao de tarefas.
- Habits: definicao e acompanhamento de habitos.
- Planner: consolidacao de rotinas e planejamento.
- Calendar: integracao com Google Calendar.

## Observacoes

- O projeto ainda esta na base inicial e deve evoluir para a arquitetura orientada a feature descrita acima.
- Quando iniciar o desenvolvimento real, remover os artefatos padrao do template da API e substituir pelos recursos do dominio LifeOS.

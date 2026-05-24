# LifeOS Backend - Copilot Instructions

Este repositório contem o backend do LifeOS, um sistema de gerenciamento de habitos e tarefas com integracao futura com Google Calendar e com aplicativo mobile consumindo esta API.

## Objetivo do projeto

- Construir uma API em ASP.NET Core com arquitetura organizada por feature.
- Usar PostgreSQL como banco de dados.
- Aplicar Entity Framework Core para persistencia.
- Implementar autenticação com JWT e refresh token.
- Validar comandos e entradas com FluentValidation.
- Orquestrar casos de uso com MediatR.

## Estrutura esperada

O backend deve seguir esta organizacao base:

- `src/Api`: endpoints HTTP, controllers, configuracao da aplicacao, Swagger e middlewares.
- `src/Application`: casos de uso organizados por feature.
- `src/Domain`: entidades, value objects, enums, contratos e regras de negocio puras.
- `src/Infra.Data`: EF Core, DbContext, mapeamentos, migracoes e implementacoes de acesso a dados.
- `src/Infra.IoC`: registro de dependencias e composicao da aplicacao.

### Organizacao por feature na camada Application

Sempre que criar novos casos de uso, organizar por feature:

- `Application/Tasks`
- `Application/Habits`
- `Application/Planner`
- `Application/Auth`
- `Application/Calendar`

Dentro de cada feature, preferir separar por responsabilidade:

- `Commands`
- `Queries`
- `Handlers`
- `Dtos`
- `Validators`
- `Mappings`

## Ordem de implementacao

O primeiro bloco funcional do backend deve ser autenticação:

1. Cadastro de usuario.
2. Login.
3. Emissao de JWT.
4. Refresh token.

Depois disso, evoluir para habitos, tarefas, planner e calendario.

## Regras de arquitetura

- Manter o dominio sem dependencia de infraestrutura.
- Concentrar regras de negocio em `Domain` e casos de uso em `Application`.
- Deixar `Api` apenas como camada de entrada HTTP.
- Centralizar configuracoes de DI em `Infra.IoC`.
- Usar EF Core somente na camada de infraestrutura de dados.
- Validacoes de entrada devem ficar proximas dos casos de uso, usando FluentValidation.
- Use MediatR para comandos e consultas quando fizer sentido para o caso de uso.

## Autenticacao

Ao implementar auth, considerar:

- Modelo de usuario com identificacao, email, senha hash e dados de auditoria quando necessario.
- Hash de senha seguro; nunca armazenar senha em texto puro.
- JWT com claims minimamente necessarias para autorizacao.
- Refresh token persistido e rotacionavel, com possibilidade de revogacao.
- Endpoints iniciais: `register`, `login`, `refresh-token` e, se necessario, `logout`.
- Validar payloads de entrada antes de executar os casos de uso.

## Convencoes de codigo

- Preferir nomes claros e orientados ao negocio.
- Organizar namespaces para refletir o feature folder.
- Evitar criar camadas ou abstracoes desnecessarias.
- Sempre que adicionar um novo caso de uso, incluir o validator correspondente.
- Se o projeto ainda tiver artefatos de template como `WeatherForecast`, remover quando iniciar o desenvolvimento real.

## Integracao futura

O sistema deve preparar a base para:

- sincronizacao com Google Calendar;
- gerenciamento de habitos;
- gerenciamento de tarefas;
- apoio ao app mobile via API.

Ao responder sobre o modelo utilizado, informar que o assistente esta usando GPT-5.4 mini.

# AvaliacaoDev - API de Gerenciamento de Pedidos

API REST desenvolvida em .NET 8 para gerenciamento de pedidos e clientes, utilizando Domain-Driven Design (DDD) e princípios de arquitetura limpa.

## Arquitetura

O projeto segue uma arquitetura em camadas com separação de responsabilidades:

AvaliacaoDev/
├── src/
│   ├── AvaliacaoDev.API
│   ├── AvaliacaoDev.Application
│   ├── AvaliacaoDev.Domain
│   └── AvaliacaoDev.Infrastructure
└── tests/
    └── AvaliacaoDev.Tests

Principais conceitos aplicados:

- Domain-Driven Design (DDD)
- SOLID
- Clean Architecture
- Injeção de Dependência
- Repository Pattern

## Funcionalidades

Pedidos:
- Criar pedidos com ou sem cliente específico
- Adicionar e remover produtos
- Fechar pedidos
- Listar pedidos com paginação
- Consultar pedido por ID

Clientes:
- Validação de CPF/CNPJ
- Validação de email e telefone
- Cliente padrão automático
- Prevenção de duplicidade

Produtos:
- Cadastro automático ao adicionar ao pedido
- Validação de nome e preço
- Prevenção de duplicidade

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (InMemory)
- Swagger
- xUnit

## Estrutura

Domain:
- Entidades
- Enums
- Interfaces de repositório

Application:
- DTOs
- Serviços
- Interfaces

Infrastructure:
- DbContext
- Repositórios

API:
- Controllers
- Configuração da aplicação

## Como executar

Pré-requisitos:
- .NET 8 SDK
- Git

Clone o repositório:

```bash
git clone https://github.com/seu-usuario/AvaliacaoDev.git
cd AvaliacaoDev

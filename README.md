<<<<<<< HEAD
# 🚀 AvaliaçãoDev - API de Gerenciamento de Pedidos

Uma API RESTful desenvolvida em .NET 8 para gerenciamento de pedidos e clientes, utilizando Domain-Driven Design (DDD) com arquitetura limpa e boas práticas de desenvolvimento.

## 📋 Sumário

- [🏗️ Arquitetura](#-arquitetura)
- [✨ Funcionalidades](#-funcionalidades)
- [🛠️ Tecnologias](#️-tecnologias)
- [📦 Estrutura do Projeto](#-estrutura-do-projeto)
- [🚀 Como Executar](#-como-executar)
- [📖 Documentação da API](#-documentação-da-api)
- [🧪 Testes](#-testes)
- [🔧 Configuração](#-configuração)
- [📝 Regras de Negócio](#-regras-de-negócio)

## 🏗️ Arquitetura

O projeto segue uma arquitetura limpa com separação de responsabilidades:

```
AvaliacaoDev/
├── src/
│   ├── AvaliacaoDev.API/          # Camada de Apresentação (Controllers)
│   ├── AvaliacaoDev.Application/  # Camada de Aplicação (Services, DTOs, Interfaces)
│   ├── AvaliacaoDev.Domain/       # Camada de Domínio (Entidades, Enums, Interfaces)
│   └── AvaliacaoDev.Infrastructure/ # Camada de Infraestrutura (Data, Repositories)
└── tests/
    └── AvaliacaoDev.Tests/        # Testes Unitários
```

### Principios Aplicados

- **Domain-Driven Design (DDD)**: Entidades ricas com regras de negócio
- **SOLID**: Princípios de design orientado a objetos
- **Clean Architecture**: Separação clara das camadas
- **Injeção de Dependência**: Desacoplamento entre componentes
- **Repository Pattern**: Abstração do acesso a dados

## ✨ Funcionalidades

### 📦 Gestão de Pedidos
- ✅ Criar pedidos (com ou sem cliente específico)
- ✅ Adicionar produtos ao pedido
- ✅ Remover produtos do pedido
- ✅ Fechar pedidos
- ✅ Listar pedidos com paginação e filtros
- ✅ Obter detalhes de um pedido específico

### 👥 Gestão de Clientes
- ✅ Validação robusta de CPF/CNPJ
- ✅ Validação de e-mail e telefone
- ✅ Cliente padrão automático
- ✅ Prevenção de duplicidade

### 📋 Gestão de Produtos
- ✅ Cadastro automático ao adicionar ao pedido
- ✅ Prevenção de duplicidade por nome e preço
- ✅ Validação de preços e nomes

## 🛠️ Tecnologias

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Framework para APIs REST
- **Entity Framework Core InMemory** - Banco de dados em memória
- **Swagger/OpenAPI** - Documentação da API
- **xUnit** - Framework de testes
- **C#** - Linguagem de programação

## 📦 Estrutura do Projeto

### Domain Layer (`AvaliacaoDev.Domain`)
Contém o coração do negócio, totalmente independente de infraestrutura:

```
Domain/
├── Entities/
│   ├── Pedido.cs      # Entidade principal com regras de negócio
│   ├── Cliente.cs     # Entidade com validações complexas
│   └── Produto.cs     # Entidade básica de produto
├── Enums/
│   └── StatusPedido.cs # Enum de status do pedido
└── Interfaces/
    ├── IPedidoRepository.cs
    ├── IClienteRepository.cs
    └── IProdutoRepository.cs
```

### Application Layer (`AvaliacaoDev.Application`)
Orquestra os casos de uso e contém a lógica de aplicação:

```
Application/
├── DTOs/
│   ├── PedidoDto.cs    # Transfer de dados para API
│   ├── ClienteDto.cs
│   └── ProdutoDto.cs
├── Services/
│   └── PedidoService.cs # Serviço principal de pedidos
└── Interfaces/
    └── IPedidoService.cs
```

### Infrastructure Layer (`AvaliacaoDev.Infrastructure`)
Implementação concreta de repositórios e acesso a dados:

```
Infrastructure/
├── Data/
│   └── AppDbContext.cs # Contexto do EF Core
└── Repositories/
    ├── PedidoRepository.cs
    ├── ClienteRepository.cs
    └── ProdutoRepository.cs
```

### API Layer (`AvaliacaoDev.API`)
Expõe os endpoints HTTP com documentação Swagger:

```
API/
├── Controllers/
│   └── PedidoController.cs # Endpoints REST
└── Program.cs              # Configuração e startup
```

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- Visual Studio 2022 ou VS Code
- Git

### Passos

1. **Clone o repositório:**
```bash
git clone https://github.com/seu-usuario/AvaliacaoDev.git
cd AvaliacaoDev
```

2. **Restaure os pacotes NuGet:**
```bash
dotnet restore
```

3. **Execute a aplicação:**
```bash
dotnet run --project src/AvaliacaoDev.API
```

4. **Acesse a API:**
- URL base: `https://localhost:7123`
- Documentação Swagger: `https://localhost:7123/swagger`

### Executar Testes
```bash
dotnet test
```

## 📖 Documentação da API

A documentação completa está disponível através do Swagger UI:

### Endpoints Principais

#### 📝 Criar Pedido
```http
POST /api/pedidos
```
- **Descrição**: Cria um novo pedido
- **Parâmetros**: `clienteId` (opcional) - ID do cliente específico
- **Resposta**: ID do pedido criado

#### 📦 Adicionar Produto
```http
POST /api/pedidos/{id}/produtos
```
- **Descrição**: Adiciona um produto ao pedido
- **Body**: ProdutoDto com nome, preço e quantidade

#### 🗑️ Remover Produto
```http
DELETE /api/pedidos/{id}/produtos/{produtoId}
```
- **Descrição**: Remove um produto do pedido

#### 🔒 Fechar Pedido
```http
POST /api/pedidos/{id}/fechar
```
- **Descrição**: Fecha o pedido impedindo alterações

#### 📋 Listar Pedidos
```http
GET /api/pedidos
```
- **Descrição**: Lista pedidos com paginação
- **Parâmetros**: `page`, `pageSize`, `status`

#### 🔍 Obter Pedido
```http
GET /api/pedidos/{id}
```
- **Descrição**: Obtém detalhes completos de um pedido

## 🧪 Testes

O projeto inclui testes unitários cobrindo:

- ✅ Validações de domínio
- ✅ Regras de negócio
- ✅ Fluxos de pedidos
- ✅ Validação de CPF/CNPJ
- ✅ Validação de e-mail e telefone

### Executar Testes
```bash
# Executar todos os testes
dotnet test

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 🔧 Configuração

### Configuração do Swagger
O Swagger está configurado para gerar documentação automática a partir dos comentários XML:

```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "API de Pedidos e Clientes", 
        Version = "v1",
        Description = "API RESTful para gerenciamento de pedidos e clientes."
    });
});
```

### Banco de Dados
Atualmente utiliza Entity Framework Core com banco em memória para simplificação:

```csharp
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("PedidosDb"));
```

## 📝 Regras de Negócio

### 🎯 Regras de Pedidos
- Todo pedido deve ter um cliente associado
- Pedido não pode ser fechado sem produtos
- Pedido fechado não pode ser alterado
- Quantidade deve ser maior que zero

### 👤 Regras de Clientes
- CPF/CNPJ deve ser válido e único
- Email deve ter formato válido e ser único
- Telefone deve seguir formato brasileiro
- Cliente padrão criado automaticamente

### 📦 Regras de Produtos
- Nome é obrigatório (máximo 200 caracteres)
- Preço deve ser positivo (máximo R$ 999.999,99)
- Prevenção de duplicidade por nome e preço

### 🔐 Validações Implementadas
- **CPF**: Validação de dígitos verificadores e bloqueio de sequências
- **CNPJ**: Validação de dígitos verificadores e bloqueio de sequências
- **Email**: Validação de formato com regex
- **Telefone**: Formato `(XX) XXXXX-XXXX` ou `XXXXX-XXXX`

## 🎯 Destaques Técnicos

### 🏆 Padrões e Práticas
- **Domain-Driven Design**: Entidades ricas com comportamentos
- **Repository Pattern**: Abstração do acesso a dados
- **DTO Pattern**: Separação entre dados de domínio e API
- **Dependency Injection**: Inversão de controle
- **Async/Await**: Programação assíncrona
- **Exception Handling**: Tratamento centralizado de erros

### 🔒 Segurança e Validação
- Validação robusta de CPF/CNPJ brasileiros
- Prevenção contra sequências conhecidas
- Validação de duplicidade em múltiplos níveis
- Cache thread-safe para cliente padrão
- Tratamento de race conditions

### 📊 Performance
- Cache de cliente padrão
- Busca otimizada de produtos
- Paginação eficiente
- Consultas normalizadas

## 🤝 Contribuindo

1. Fork o projeto
2. Crie sua feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para o branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👨‍💻 Autor

**Seu Nome**  
Desenvolvedor .NET  
[LinkedIn](https://linkedin.com/in/seu-perfil) | [GitHub](https://github.com/seu-usuario)

---

⭐ **Se este projeto foi útil, deixe uma estrela!**
=======
# AvaliacaoDev
>>>>>>> 6479b9e170dab454e806ccd161a489761ab83cd9

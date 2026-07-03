📦 Order Management API

API REST desenvolvida em .NET 10 para gerenciamento de clientes, produtos, estoque e pedidos.

Este projeto foi construído como desafio técnico, aplicando Clean Architecture, DDD e boas práticas de backend, com foco em regras de negócio consistentes, separação de responsabilidades e testabilidade.

🚀 Visão geral

A API permite:

Gestão de clientes e produtos
Controle de estoque
Criação e gerenciamento de pedidos
Controle de status de pedidos com regras de transição
Auditoria completa de alterações (histórico)
Validações de regras de negócio no domínio
Persistência em banco relacional
🧱 Tecnologias
.NET 10
ASP.NET Core Web API
Entity Framework Core
SQL Server
FluentValidation
Swagger / OpenAPI
xUnit
Moq
FluentAssertions
🏗️ Arquitetura

O projeto segue Clean Architecture + DDD, dividido em:

Domain → regras de negócio e entidades
Application → casos de uso (services)
Infrastructure → persistência e EF Core
API → endpoints HTTP

Essa separação garante:

isolamento de regras de negócio
alta testabilidade
baixo acoplamento
facilidade de manutenção
📁 Estrutura do projeto
src
├── OrderManagement.API
│   ├── Controllers
│   └── Program.cs
│
├── OrderManagement.Application
│   ├── Contracts
│   ├── Services
│   ├── Interfaces
│   ├── Mappings
│   └── Validators
│
├── OrderManagement.Domain
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
└── OrderManagement.Infrastructure
    ├── Context
    ├── Repositories
    ├── Configurations
    └── Migrations

tests
└── OrderManagement.UnitTests
📌 Principais decisões técnicas
✔ Contracts ao invés de DTOs

Separação clara entre entrada/saída da API sem acoplamento ao domínio.

✔ Domínio rico

Regras de negócio são centralizadas nas entidades, evitando “anemic domain model”.

✔ Controle de estoque no domínio

Estoque é validado e atualizado durante criação de pedidos.

✔ Decimal para valores monetários

Evita problemas de precisão de ponto flutuante.

✔ Datas em UTC

Todas as datas são persistidas em UTC (DateTime.UtcNow).

A conversão para o fuso America/Sao_Paulo deve ser feita na camada de apresentação.

✔ Enumerações

Usadas para controle de estado de:

Cliente
Produto
Pedido
🔄 Fluxo de pedidos
Status suportados:
Created
Paid
Shipped
Cancelled
Regras:
Created → Paid
Paid → Shipped
Created → Cancelled
Restrições:
Pedido enviado não pode ser cancelado
Pedido cancelado não pode ser alterado
Transições inválidas são bloqueadas no domínio
📜 Histórico de pedidos

O sistema mantém auditoria completa de mudanças de status.

Cada alteração registra:

Status anterior
Novo status
Data/hora (UTC)
Motivo (opcional)
📡 Endpoints
Customers
POST /api/customers
GET /api/customers
GET /api/customers/{id}
PATCH /api/customers/{id}/status
Products
POST /api/products
GET /api/products
GET /api/products/{id}
PUT /api/products/{id}
PATCH /api/products/{id}/status
PATCH /api/products/{id}/stock
Orders
POST /api/orders
GET /api/orders
GET /api/orders/{id}
PATCH /api/orders/{id}/status
📄 Regras de negócio principais
Clientes inativos não podem criar pedidos
Produtos inativos não podem ser usados em pedidos
Estoque não pode ser negativo
Pedido deve conter pelo menos um item
Estoque é validado antes da criação do pedido
Estoque é debitado na criação do pedido
Cancelamento retorna estoque (se permitido)
Preço do item é fixado no momento da compra
📊 Paginação

Endpoints de listagem não possuem paginação implementada no momento.

Justificativa:

A implementação foi simplificada para foco nas regras de domínio e fluxo de pedidos.

Evolução futura:

Em um cenário de produção seria aplicada:

PageNumber
PageSize
Skip / Take
TotalCount
⚔️ Concorrência de estoque

Atualmente, a aplicação não implementa controle de concorrência otimista (RowVersion).

Comportamento atual:
Validação de estoque ocorre antes da baixa
Operações são feitas via Unit of Work (transação lógica)
Limitação:

Em cenários de alta concorrência pode ocorrer race condition.

Evolução recomendada:
RowVersion (EF Core concurrency token)
Lock otimista/pessimista no banco
ou isolamento transacional mais forte
💰 Valores monetários

Valores financeiros são representados com decimal para garantir precisão e evitar erros de arredondamento.

🕒 Datas e fuso horário
Persistência em UTC
Uso de DateTime.UtcNow
Conversão para America/Sao_Paulo na camada de apresentação
🧪 Testes

O projeto utiliza:

xUnit (framework)
Moq (mock de dependências)
FluentAssertions (assertividade)
Estrutura
Application
Orders
Products
Customers
O que é testado
Criação de pedidos
Validação de estoque
Regras de clientes e produtos ativos/inativos
Fluxo de status
Cancelamento de pedidos
Consistência de estoque
Histórico de pedidos
Estratégia
Testes focados em regras de negócio
Isolamento com mocks
Validação de comportamento (não implementação)
Execução
dotnet test
Observação

Nem todos os endpoints possuem cobertura completa, pois o foco está nas regras críticas de domínio.

🧩 Swagger

A API possui documentação via Swagger/OpenAPI com:

schemas tipados
endpoints testáveis
respostas HTTP documentadas

Disponível em:

/swagger
📌 Requisitos não funcionais
.NET 10
API REST
EF Core + SQL Server
Validações com FluentValidation
Arquitetura em camadas
Uso de UTC
Uso de decimal para dinheiro
Testes automatizados
📈 Roadmap
Concluído
Arquitetura base
Domínio e regras de negócio
EF Core + migrations
API completa
Testes unitários
Swagger
Tratamento global de exceções
Futuro
Docker
Paginação completa
Concorrência otimista (RowVersion)
Melhorias de performance em queries
🧠 Decisões técnicas e trade-offs
EF Core

Escolhido por produtividade e integração com .NET.

Unit of Work

Garantia de consistência transacional.

Sem concorrência otimista

Trade-off de simplicidade vs robustez em alta carga.

Sem paginação

Foco em domínio e regras do negócio.

Testes focados em regras

Cobertura prioriza regras críticas ao invés de endpoints.

👤 Autor

Cesar Danziger
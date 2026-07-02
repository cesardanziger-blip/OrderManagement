# Order Management API

> Projeto desenvolvido para fins de avaliação técnica, seguindo as especificações fornecidas no desafio.

API REST desenvolvida em .NET 10 para gerenciamento de clientes, produtos, estoque e pedidos, aplicando princípios de Clean Architecture, DDD (Domain-Driven Design) e boas práticas de desenvolvimento backend.
O projeto foi estruturado com foco em clareza de domínio, consistência de regras de negócio e separação de responsabilidades.

---

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- FluentValidation
- Swagger / OpenAPI
- xUnit (testes unitários)

---

## Arquitetura

O projeto foi organizado seguindo princípios de **Domain-Driven Design (DDD)**, **SOLID** e separação de responsabilidades.

---

## Estrutura do Projeto

```
src
├── OrderManagement.API # Endpoints e configuração da API
│   ├── Controllers
│   └── Program.cs
│
├── OrderManagement.Application # Casos de uso e regras da aplicação
│   ├── Contracts
│   │   ├── Requests
│   │   └── Responses
│   ├── Services
│   ├── Interfaces
│   ├── Mappings
│   └── Validators
│
├── OrderManagement.Domain  # Entidades, regras de negócio e contratos
│   ├── Entities
│   ├── Enums
│   └── Interfaces
│
└── OrderManagement.Infrastructure # Persistência, Entity Framework e SQL Server
    ├── Context
    ├── Repositories
    ├── Configurations
    └── Migrations

tests
└── OrderManagement.UnitTests # Testes automatizados
```
---

## Princípios adotados

- Domain-Driven Design (DDD)
- SOLID
- Clean Architecture
- Clean Code
---

## Principais decisões técnicas

- Uso de Contracts ao invés de DTOs para separar entrada e saída da API
- Domínio isolado de frameworks externos
- Regras de negócio centralizadas nas entidades
- Controle de estoque realizado dentro do domínio
- Uso de decimal para valores monetários
- Datas sempre em UTC (DateTime.UtcNow)
- Enumerações para controle de estado (Customer, Product, Order)

### Fluxo de pedidos
O sistema implementa um fluxo controlado de status de pedidos:

Fluxo de pedidos:
- Created → Paid → Shipped
- Created → Cancelled

Regras:
Transições inválidas são bloqueadas no domínio
Histórico completo de mudanças é registrado via OrderHistory
Status controlado exclusivamente pela entidade Order

### Histórico de pedidos

O sistema mantém auditoria completa de alterações de status.

Registro criado automaticamente no momento da criação do pedido
Histórico inclui:
Status anterior
Novo status
Data da alteração
Motivo (quando aplicável)

## Executando o projeto

### Pré-requisitos
- .NET 10 SDK
- SQL Server

### Validação

O projeto utiliza FluentValidation na camada de Application.

Responsabilidades:
- Validação de entrada (requests)
- Regras de formato e consistência
- Validações assíncronas (ex: unicidade de email/documento)

Separação de responsabilidades:
- FluentValidation → validação de entrada
- Domain → regras de negócio

Regras:
- Transições inválidas são bloqueadas no domínio
- Histórico completo de mudanças é registrado via OrderHistory
- Status controlado exclusivamente pela entidade Order

---
## Requisitos não funcionais

- Uso de EF Core como ORM principal
- Uso de SQL Server como banco relacional
- Arquitetura em camadas para separação de responsabilidades
- Uso de decimal para precisão financeira
- Uso de UTC para padronização de datas

### Swagger / Documentação

A API é documentada com Swagger/OpenAPI.

Funcionalidades:
- Documentação automática dos endpoints
- Testes via interface interativa
- Definição explícita de status HTTP
- Schema de requests e responses claramente tipados

Disponível em:
/swagger

---
###  Endpoints principais

Customers
POST /api/customers
GET /api/customers
GET /api/customers/{id}

Products
POST /api/products
GET /api/products
PATCH /api/products/{id}/status
PATCH /api/products/{id}/stock

Orders
POST /api/orders
GET /api/orders
GET /api/orders/{id}
PATCH /api/orders/{id}/status

## Testes

Testes unitários implementados com foco em regras de domínio.

Cobertura prevista:

- Regras de pedidos
- Validação de estoque
- Transições de status
- Regras de negócio críticas

---

## Destaques Técnicos

- Modelo de domínio rico com regras de negócio encapsuladas nas entidades
- Controle de estoque garantido durante criação de pedidos
- Fluxo de status de pedidos com regras de transição controladas no domínio
- Histórico completo de alterações de status para auditoria
- Separação clara entre validação de entrada (FluentValidation) e regras de negócio (Domain)

## Roadmap

### Estrutura inicial
- [x] Estrutura da solução
- [x] Configuração da arquitetura
- [x] Configuração do Entity Framework Core

### Domínio
- [x] Modelagem das entidades
- [x] Implementação das regras de negócio
- [x] Implementação do fluxo de status dos pedidos

### Persistência
- [x] Configuração das entidades (EF Core)
- [x] Repositórios
- [x] Migrations

### Aplicação
- [x] Casos de uso
- [x] Mapeamentos
- [x] Contracts (Requests e Responses)
- [x] Services
- [x] Validações com FluentValidation

### API
- [x] Endpoints de clientes
- [x] Endpoints de produtos
- [x] Endpoints de pedidos
- [x] Swagger

### Testes
- [ ] Testes unitários
- [ ] Testes de regras de negócio

### Infraestrutura
- [ ] Docker
- [ ] Tratamento global de exceções

### Regras de negócio principais
Clientes inativos não podem criar pedidos
Produtos inativos não podem ser utilizados
Estoque é validado e atualizado no momento do pedido
Status de pedido segue fluxo controlado
Histórico de alterações é obrigatório

### Observação final
Este projeto foi desenvolvido com foco em:

- Clareza de domínio
- Boas práticas de arquitetura
- Simplicidade com consistência
- Facilidade de manutenção e evolução

### Executar aplicação

```bash
dotnet restore
dotnet build
dotnet run --project src/OrderManagement.API
```

## Autor
Cesar Danziger
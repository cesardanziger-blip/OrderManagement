# Order Management API

> Projeto desenvolvido para fins de avaliação técnica, seguindo as especificações fornecidas no desafio.

API REST desenvolvida como solução para o desafio técnico da TWRT.

O objetivo da aplicação é gerenciar clientes, produtos, estoque e pedidos, aplicando boas práticas de arquitetura, modelagem de domínio e desenvolvimento backend utilizando .NET.

---

## Tecnologias

- .NET 10
- ASP.NET Core Web API

---

## Arquitetura

O projeto foi organizado seguindo princípios de **Domain-Driven Design (DDD)**, **SOLID** e separação de responsabilidades.

```
src
├── OrderManagement.API              # Endpoints e configuração da API
├── OrderManagement.Application      # Casos de uso e regras da aplicação
├── OrderManagement.Domain           # Entidades, regras de negócio e contratos
└── OrderManagement.Infrastructure   # Persistência, Entity Framework e SQL Server

tests
└── OrderManagement.UnitTests        # Testes automatizados
```

---
## Princípios adotados

- Domain-Driven Design (DDD)
- SOLID
- Clean Architecture
- Clean Code
---

## Estrutura do Projeto

```
OrderManagement
│
├── src
│   ├── OrderManagement.API
│   ├── OrderManagement.Application
│   ├── OrderManagement.Domain
│   └── OrderManagement.Infrastructure
│
├── tests
│   └── OrderManagement.UnitTests
│
├── README.md
└── OrderManagement.slnx
```

---

## Executando o projeto

As instruções de execução e configuração serão documentadas conforme o desenvolvimento da aplicação evoluir.

---

## Testes

Os testes automatizados serão implementados utilizando **xUnit**, priorizando as principais regras de negócio da camada de domínio.

---

## Roadmap

### Estrutura inicial

- [x] Estrutura da solução
- [ ] Configuração da arquitetura
- [ ] Configuração do Entity Framework Core

### Domínio

- [ ] Modelagem das entidades
- [ ] Implementação das regras de negócio
- [ ] Implementação do fluxo de status dos pedidos

### Persistência

- [ ] Configuração das entidades (EF Core)
- [ ] Repositórios
- [ ] Migrations

### Aplicação

- [ ] Casos de uso
- [ ] Validações com FluentValidation
- [ ] Mapeamentos entre DTOs e entidades

### API

- [ ] Endpoints de clientes
- [ ] Endpoints de produtos
- [ ] Endpoints de pedidos
- [ ] Documentação com Swagger

### Testes

- [ ] Testes unitários
- [ ] Testes das regras de negócio

### Infraestrutura

- [ ] Docker

### Melhorias

- [ ] Tratamento global de exceções

## Autor

Cesar Danziger
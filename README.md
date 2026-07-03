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
- Moq
- FluentAssertions
- Docker

---
## Executar aplicação

### Pré-requisitos

* .NET 10 SDK
* SQL Server

### Iniciar a aplicação

Na raiz do projeto, execute:

```bash
dotnet restore
dotnet build
dotnet run --project src/OrderManagement.API
```

Após a inicialização, a API estará disponível em:

**API / Swagger**

```
http://localhost:5174/swagger
```

> **Observação:** a porta (`5174`) corresponde à configuração padrão do projeto. Caso ela seja alterada no arquivo `launchSettings.json` ou por variáveis de ambiente, a URL poderá ser diferente.

---

## Executando com Docker
### Pré-requisitos

- Docker Desktop instalado
- Docker Desktop em execução (Engine Running)

### Iniciar a aplicação

```bash
docker compose up --build
```

Na primeira execução, a API aplica automaticamente as migrations do Entity Framework Core e cria o banco de dados, caso ele ainda não exista.

### Acessar a aplicação

API

```
http://localhost:8080
```

Swagger

```
http://localhost:8080/swagger
```

### Encerrar os containers

```bash
docker compose down
```

Para remover também o volume persistente do SQL Server:

```bash
docker compose down -v
```
---
## Deploy em Ambiente Cloud (AWS)

A aplicação foi publicada em ambiente real na AWS utilizando uma instância EC2 com Docker Compose, incluindo API e banco de dados SQL Server em containers separados.

### Arquitetura do deploy

- EC2 (Ubuntu)
- Docker + Docker Compose
- API .NET 10 rodando em container
- SQL Server 2022 em container
- Exposição da API via porta 8080
- Swagger disponível publicamente para validação

### URL de acesso (temporária)
http://18.216.183.255:8080/swagger

> A instância foi utilizada exclusivamente para validação e testes da entrega técnica.

### Decisão de desligamento

Após a validação do deploy e funcionamento completo da aplicação em ambiente cloud, a instância será/desligada ou foi desligada para evitar custos adicionais de infraestrutura.

### Justificativa

O ambiente AWS foi utilizado apenas como demonstração prática de deploy, comprovando:

- Funcionamento da aplicação em container
- Integração com banco de dados SQL Server
- Execução correta das migrations
- Exposição da API em ambiente real

Após essa validação, a instância foi desligada propositalmente para controle de custos, mantendo o projeto reprodutível via Docker local.

---
###  Endpoints principais

Customers
POST /api/customers
GET /api/customers (paginado)
GET /api/customers/{id}

Products
POST /api/products
GET /api/products (paginado)
PATCH /api/products/{id}/status
PATCH /api/products/{id}/stock

Orders
POST /api/orders
GET /api/orders (paginado)
GET /api/orders/{id}
PATCH /api/orders/{id}/status

---

## Arquitetura

A solução segue princípios de:

- Domain (regras de negócio)
- Clean Architecture
- SOLID
- Clean Code

A solução foi organizada em camadas seguindo princípios de Clean Architecture e DDD:

- API: camada de entrada da aplicação (Controllers e configuração do pipeline HTTP)
- Application: casos de uso, serviços, contratos e validações
- Domain: regras de negócio, entidades e interfaces do domínio
- Infrastructure: persistência de dados, Entity Framework Core e repositórios
- Tests: testes unitários focados em regras de negócio

---

### Regras de negócio principais
Clientes inativos não podem criar pedidos
Produtos inativos não podem ser utilizados
Estoque é validado e atualizado no momento do pedido
Status de pedido segue fluxo controlado
Histórico de alterações é obrigatório

---
### Fluxo de pedidos
O sistema implementa um fluxo controlado de status de pedidos:

Fluxo de pedidos:
- Created → Paid → Shipped
- Created → Cancelled

Regras:
Transições inválidas são bloqueadas no domínio
Histórico completo de mudanças é registrado via OrderHistory
Status controlado exclusivamente pela entidade Order

---

### Controle de estoque

O controle de estoque é feito diretamente na camada de aplicação/domínio durante a criação de pedidos.

Garantias:
- Validação de estoque antes da criação do pedido
- Baixa de estoque apenas após validação de todos os itens
- Cancelamento retorna estoque apenas se permitido pelas regras de negócio

Trade-off:
- Não foi implementado controle de concorrência otimista (RowVersion)
- Em cenários reais de alta concorrência, isso seria necessário para evitar race conditions

---
### Histórico de pedidos

O sistema mantém auditoria completa de alterações de status.

Registro criado automaticamente no momento da criação do pedido
Histórico inclui:
Status anterior
Novo status
Data da alteração
Motivo (quando aplicável)

---
## Principais decisões técnicas

- Uso de Contracts ao invés de DTOs para separar claramente as camadas de entrada e saída da API
- Domínio isolado de frameworks externos, mantendo regras de negócio independentes da infraestrutura
- Regras de negócio centralizadas nas entidades, seguindo princípios de Domain-Driven Design (DDD)
- Controle de estoque realizado dentro do domínio durante o fluxo de criação e cancelamento de pedidos
- Uso do tipo decimal para representar valores monetários com precisão
- Datas armazenadas em UTC (DateTime.UtcNow) para garantir consistência temporal
- Uso de enums para controle de estado (Customer, Product e Order), reduzindo estados inválidos

---

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
### Persistência de dados

Foi utilizado Entity Framework Core com SQL Server como ORM e banco relacional.

Trade-off:
- EF Core facilita produtividade e abstração de queries
- Em contrapartida, pode esconder complexidade de queries mais críticas

A escolha foi feita visando produtividade e aderência ao escopo do desafio.

---

### Consistência transacional

A consistência entre criação de pedido e atualização de estoque é garantida pelo uso de Unit of Work.

Isso garante que:
- Ou todas as operações são persistidas
- Ou nenhuma alteração é aplicada

---

### Valores monetários

Valores financeiros são representados utilizando decimal, garantindo precisão e evitando problemas de arredondamento comuns com tipos de ponto flutuante.

---

### Datas e fuso horário

As datas são armazenadas em UTC (DateTime.UtcNow) para garantir consistência no banco de dados.

A interpretação para o fuso America/Sao_Paulo deve ser feita na camada de apresentação.

---

### Extensões e configuração global

O projeto utiliza extension methods para centralizar a configuração da aplicação, reduzindo acoplamento no Program.cs.

Exemplos:
- Configuração de dependências por camada (Application / Infrastructure)
- Registro de serviços e repositórios via métodos de extensão
- Middleware global de tratamento de exceções

Essa abordagem mantém o Program.cs limpo e melhora a organização da inicialização da aplicação.

---

### Testes automatizados

Foram implementados testes unitários com foco em regras de negócio críticas, totalizando 40 cenários validados.

Estratégia adotada:
- Isolamento de dependências com Moq
- Validação de comportamento do domínio
- Cobertura dos fluxos mais sensíveis (estoque, pedidos e status)

Os testes priorizam regras de negócio ao invés de cobertura completa de endpoints.

Trade-off:
- Não foi priorizada cobertura completa de toda a camada de aplicação e infraestrutura
- Foco em consistência de regras críticas do domínio

---

### Paginação

Todos os endpoints de listagem (Customers, Products e Orders) possuem suporte a paginação.

A paginação foi implementada utilizando os parâmetros:

- PageNumber (padrão: 1)
- PageSize (padrão: 10)

A resposta segue um padrão estruturado contendo:

- Itens da página atual
- Total de registros
- Página atual
- Tamanho da página
- Informações auxiliares (TotalPages, HasNext, HasPrevious)

Exemplo de uso:

GET /api/orders?pageNumber=1&pageSize=10

Estratégia adotada:

- Uso de Skip e Take no Entity Framework Core
- Cálculo de total de registros para controle de paginação
- Estrutura de resposta padronizada para reutilização em diferentes entidades

Essa abordagem garante melhor performance em listagens e evita carregamento completo de dados em memória.

## Requisitos não funcionais

- Uso de EF Core como ORM principal
- Uso de SQL Server como banco relacional
- Arquitetura em camadas para separação de responsabilidades
- Uso de decimal para precisão financeira
- Uso de UTC para padronização de datas

---
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
## Destaques Técnicos

- Modelo de domínio rico com regras de negócio encapsuladas nas entidades
- Controle de estoque garantido durante criação de pedidos
- Fluxo de status de pedidos com regras de transição controladas no domínio
- Histórico completo de alterações de status para auditoria
- Separação clara entre validação de entrada (FluentValidation) e regras de negócio (Domain)

---

### Como executar os testes

Na raiz do projeto:

```bash
dotnet test
```
---

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
- [x] Testes unitários (isolamento de serviços e validação de regras individuais)
- [x] Testes de regras de negócio (validação de comportamento do domínio)

### Infraestrutura
- [x] Docker
- [x] Tratamento global de exceções

---

### Observação final
Este projeto foi desenvolvido com foco em:

- Clareza de domínio
- Boas práticas de arquitetura
- Simplicidade com consistência
- Facilidade de manutenção e evolução

---
## Autor
Cesar Danziger
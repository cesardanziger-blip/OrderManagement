# OrderManagement API - TECHNICAL DECISIONS

## Architecture

O projeto segue uma arquitetura em camadas:

- Domain: entidades e regras de negócio
- Application: casos de uso, contratos e orquestração
- Infrastructure: persistência com EF Core
- API: exposição dos endpoints
- Testes: testes unitários e de integração

## Domain Design

O domínio foi modelado utilizando entidades ricas, onde as regras de negócio estão encapsuladas nas próprias entidades, evitando anemização do modelo.

Principais decisões:
- Uso de enums para estados (CustomerStatus, OrderStatus, ProductStatus)
- Controle de fluxo de pedido via transições explícitas
- Histórico de alterações de status em OrderHistory
- Cálculo de total realizado dentro da entidade Order
- Controle de estoque dentro da entidade Product

## Time & Money Strategy
### Datas

- Todos os timestamps são persistidos em UTC utilizando `DateTime.UtcNow`.
- Durante o mapeamento para os contratos de resposta, as datas são convertidas para o fuso horário `America/Sao_Paulo` através de uma extensão (`DateTimeExtensions`).
- Essa abordagem mantém a persistência independente do ambiente de execução e atende ao requisito de exibição das datas no horário local.

### Valores monetários

- Valores monetários são representados utilizando o tipo `decimal`, evitando perda de precisão inerente aos tipos de ponto flutuante.
- Todos os cálculos de totais dos pedidos são realizados pela aplicação, garantindo consistência entre os itens e o valor final do pedido.

## Persistence Strategy (EF Core)

O projeto utiliza Entity Framework Core com Fluent API para configuração das entidades, separando o mapeamento da camada de domínio.

Decisões principais:
- Configurações isoladas por entidade utilizando `IEntityTypeConfiguration<T>`
- Uso de `HasConversion<int>()` para persistência de enums como inteiros, reduzindo o espaço de armazenamento e desacoplando a representação do banco da forma como os dados são expostos pela API.
- Definição de índices únicos para Email e Document, considerando apenas registros ativos
- Os enums são serializados como texto nas respostas da API utilizando `JsonStringEnumConverter`, mantendo a persistência otimizada e tornando o contrato da API mais legível.

## Decisões arquiteturais

- DTOs foram substituídos por Contracts para separar claramente Requests e Responses
- Application Layer centraliza regras de orquestração de casos de uso
- Domain contém apenas regras de negócio e entidades
- Mapeamentos foram isolados via extension methods
- Infrastructure é responsável exclusivamente por persistência
- API não acessa Domain diretamente, apenas Application Layer
- Conversões de entidades para contratos foram centralizadas em extension methods, incluindo formatação de enums e conversão de datas para o fuso horário da aplicação.

## Validation strategy

O projeto utiliza FluentValidation na Application Layer para validação das entradas da API.

Decisões principais:

Validações centralizadas por caso de uso (ex: CreateCustomerValidator)
Regras de input separadas da lógica de negócio
Uso de validações síncronas e assíncronas (ex: unicidade de Email e Documento)
Integração com MediatR via Pipeline Behavior, garantindo validação automática antes da execução dos handlers
Reutilização de validações comuns (ex: CPF/CNPJ) em classe compartilhada na camada Application

Benefícios:

Redução de lógica de validação nos services/handlers
Padronização de erros de entrada
Maior clareza e separação de responsabilidades
Facilidade de manutenção e evolução das regras

## API Documentation (Swagger/OpenAPI)

O projeto utiliza Swagger/OpenAPI para documentação interativa da API.

Decisões:
- Exposição automática dos endpoints via Swashbuckle
- Uso de XML comments para enriquecer a documentação
- Definição explícita de status codes com ProducesResponseType
- Projeto configurado para gerar o xml automaticamente
- Documentação de request/response schemas

Benefícios:
- Facilita testes manuais da API
- Melhora a clareza dos contratos
- Permite validação visual das respostas HTTP

## Order Status Management

O gerenciamento de status de pedidos é realizado através de um único endpoint:

PATCH /api/orders/{id}/status

Decisão arquitetural:
- Centralização da mudança de estado em um único endpoint
- Uso de enum OrderStatus para representar o estado desejado
- Delegação das regras de transição para a entidade Order

Fluxo permitido:
- Created → Paid
- Paid → Shipped
- Created → Cancelled

Benefícios:
- Evita múltiplos endpoints redundantes (Pay, Ship, Cancel)
- Centraliza regras de transição de estado no domínio
- Facilita manutenção e evolução do fluxo de pedidos

## Order History Strategy

O sistema mantém um histórico completo das alterações de status dos pedidos através da entidade OrderHistory.

Decisão na criação do pedido:
- Um registro inicial de histórico é criado no momento da criação do pedido
- O status inicial é "Created → Created" para garantir rastreabilidade desde o início do ciclo de vida do pedido

Alternativa considerada:
- Não registrar histórico na criação e iniciar apenas em transições futuras

Decisão adotada:
- Registro inicial mantido para garantir auditoria completa desde a origem do pedido

Benefícios:
- Rastreabilidade completa do ciclo de vida do pedido
- Consistência na auditoria de estados
- Facilidade de debugging e análise de fluxo

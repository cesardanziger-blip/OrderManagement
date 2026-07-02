# OrderManagement API - TECHNICAL DECISIONS

## Arquitetura

O projeto segue uma arquitetura em camadas:

- Domain: entidades e regras de negócio
- Application: casos de uso, contratos e orquestração
- Infrastructure: persistência com EF Core
- API: exposição dos endpoints
- Testes: testes unitários e de integração

## Domínio

O domínio foi modelado utilizando entidades ricas, onde as regras de negócio estão encapsuladas nas próprias entidades, evitando anemização do modelo.

Principais decisões:
- Uso de enums para estados (CustomerStatus, OrderStatus, ProductStatus)
- Controle de fluxo de pedido via transições explícitas
- Histórico de alterações de status em OrderHistory
- Cálculo de total realizado dentro da entidade Order
- Controle de estoque dentro da entidade Product
- Stock é valor absoluto
- SetStock substitui UpdateStock
- OrderService usa transaction para não debitar ou aumentar o stock sem o pedido houver sido concluido

## Estratégia Data e Dinheiro
### Datas

- Todos os timestamps são persistidos em UTC utilizando `DateTime.UtcNow`.
- Durante o mapeamento para os contratos de resposta, as datas são convertidas para o fuso horário `America/Sao_Paulo` através de uma extensão (`DateTimeExtensions`).
- Essa abordagem mantém a persistência independente do ambiente de execução e atende ao requisito de exibição das datas no horário local.

### Valores monetários

- Valores monetários são representados utilizando o tipo `decimal`, evitando perda de precisão inerente aos tipos de ponto flutuante.
- Todos os cálculos de totais dos pedidos são realizados pela aplicação, garantindo consistência entre os itens e o valor final do pedido.

## Estratégia de persistência (EF Core)

O projeto adota uma estratégia de persistência baseada no padrão Repository + Unit of Work, utilizando o mecanismo de rastreamento de alterações (Change Tracking) do Entity Framework Core.

Decisões principais:
- Configurações isoladas por entidade utilizando `IEntityTypeConfiguration<T>`
- Uso de `HasConversion<int>()` para persistência de enums como inteiros, reduzindo o espaço de armazenamento e desacoplando a representação do banco da forma como os dados são expostos pela API.
- Definição de índices únicos para Email e Document, considerando apenas registros ativos
- Os enums são serializados como texto nas respostas da API utilizando `JsonStringEnumConverter`, mantendo a persistência otimizada e tornando o contrato da API mais legível.

## Fluxo de persistência
Durante a criação de um pedido, a aplicação executa o seguinte fluxo:

Validação do cliente.
Validação dos produtos.
Atualização do estoque em memória.
Criação da entidade Order.
Persistência de todas as alterações através de uma única chamada ao UnitOfWork.

Essa abordagem garante que todas as alterações sejam persistidas em uma única operação, reduzindo o risco de inconsistências caso ocorra alguma falha durante o processamento do pedido.

## Decisões arquiteturais

- DTOs foram substituídos por Contracts para separar claramente Requests e Responses
- Application Layer centraliza regras de orquestração de casos de uso
- Domain contém apenas regras de negócio e entidades
- Mapeamentos foram isolados via extension methods
- Infrastructure é responsável exclusivamente por persistência
- API não acessa Domain diretamente, apenas Application Layer
- Conversões de entidades para contratos foram centralizadas em extension methods, incluindo formatação de enums e conversão de datas para o fuso horário da aplicação.
- Os repositórios são responsáveis apenas por consultas e manipulação das entidades, não realizando persistência das alterações.
- O método SaveChangesAsync() foi centralizado na abstração IUnitOfWork, tornando a camada de aplicação responsável por definir o momento do commit da operação.
- Como todas as entidades são carregadas pelo mesmo DbContext durante a requisição, o Entity Framework Core rastreia automaticamente as alterações realizadas nas entidades, dispensando chamadas explícitas de Update() para objetos já carregados pelo contexto.
- A criação de pedidos e as alterações de estoque passaram a ser persistidas através de uma única operação de SaveChangesAsync(), evitando estados parcialmente persistidos.

## Estratégia de validação

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

## API Documentação (Swagger/OpenAPI)

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

## Customer Status Gerenciamento

O gerenciamento de status do cliente é realizado através de regras encapsuladas na própria entidade `Customer`, seguindo o conceito de entidade rica.

Decisão arquitetural:
- O status do cliente é representado por um enum (`CustomerStatus`), garantindo tipagem forte e clareza de domínio
- As transições de estado são controladas exclusivamente pela entidade, através dos métodos `Activate()` e `Deactivate()`
- Não é permitido alterar diretamente o status sem passar pelas regras de domínio

Fluxo permitido:
- Inactive → Active
- Active → Inactive

Regras de domínio:
- Um cliente ativo não pode ser ativado novamente
- Um cliente inativo não pode ser desativado novamente
- Tentativas de transição inválida são bloqueadas pela entidade e resultam em exceção de regra de negócio

Benefícios:
- Garante consistência do estado da entidade
- Evita alterações diretas sem validação das regras de domínio
- Centraliza regras de negócio dentro da entidade Customer
- Facilita testes unitários da lógica de estado
- Reduz dependência de lógica de validação na camada de aplicação

## Order Status Gerenciamento

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

## Estratégia histórico de pedido

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

## Controle de estoque
O controle de estoque foi ajustado para tratar o valor informado como o estoque atual do produto, e não como uma variação incremental.

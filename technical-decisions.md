# OrderManagement API - TECHNICAL DECISIONS

## Architecture

O projeto segue uma arquitetura em camadas:

- Domain: entidades e regras de negócio
- Application: casos de uso
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

- Datas utilizam UTC (`DateTime.UtcNow`)
- Valores monetários são representados com `decimal` para precisão
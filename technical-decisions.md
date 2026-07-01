## Domain Model Design - Customer Validation

A validação do Customer foi encapsulada dentro da própria entidade, através de um método interno `Validate()`.

Essa decisão foi tomada para manter o domínio consistente e evitar que regras de negócio fiquem dispersas em services ou camada de aplicação.

A entidade passa a ser responsável por garantir sua própria integridade, seguindo o conceito de Domain Model rico.

Esse padrão pode ser aplicado em outras entidades do domínio, garantindo consistência na responsabilidade de validação.


## Audit Fields Strategy

O método `Touch()` centraliza a atualização do campo `UpdatedAt`, garantindo consistência e evitando duplicação de lógica em diferentes pontos da aplicação.

O padrão pode ser aplicado nas demais entidades do domínio para manter uniformidade na estratégia de auditoria.


## Domain Modeling - Status Representation

O status de Customer foi modelado utilizando um enum (`CustomerStatus`) ao invés de valores livres (strings ou números isolados).

Essa decisão visa garantir tipagem forte, evitar estados inválidos e melhorar a legibilidade e manutenção do código.

O padrão pode ser aplicado em outras entidades do domínio que possuam estados, garantindo consistência na modelagem.

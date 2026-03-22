# ADR 0004: Mapeamento do campo _id do MongoDB na infraestrutura

Status: Aceito
Data: 2026-03-22

## Contexto
- Registrar o mapeamento da classe Order sem contaminar o domínio com regras de banco de dados.

## Problema
- Ao testar o ambiente produtivo, a API de busca de pedidos retornou erro 500, pois a aplicação não conseguiu encontrar a propriedade da classe correspondente ao campo "_id" do documento no MongoDB.

## Decisão
1. Atualizei o método `AddMongoDbConfig` adicionando um mapeamento para a classe `Order`, ignorando elementos extras:
   ```
   cm.SetIgnoreExtraElements(true);
   ```
2. Não modificar a classe `Order` da camada de domínio, seja adicionando anotações ou nova propriedade "id"/"_id".

## Justificativa
- **Camada Infraestrutura vs Domínio**: O domínio não conhece e nem depende do banco de dados para existir, mas se adicionar algum pacote do MongoDB ali para fazer o mapeamento direto na classe, quebraria justamente o princípio de isolamento do domínio, além da forte acoplamento de requisitos de banco de dados no domínio.
- Mantém o domínio puro, sem dependências de infraestrutura, facilitando testes e evolução independente.

## Consequências
- **Facilidade de implementação**: Adicionar o mapeamento na camada de infraestrutura foi simples e rápido.
- **Sem necessidade de refatoração**: Como a classe de domínio não foi modificada, não precisei modificar ou refatorar os demais métodos que usam ou dependem da classe `Order`.
- **Risco de mascaramento**: Pode mascarar erros de mapeamento se campos extras não forem monitorados adequadamente.

## Alternativas consideradas
- **Adicionar propriedade id** — Rejeitado: Além de ser um campo do qual a regra de negócio em si não depende, estaria trazendo informações desnecessárias do banco de dados.
- **Adicionar anotações à classe** — Rejeitado: Teria que instalar pacote do MongoDB na camada de domínio.
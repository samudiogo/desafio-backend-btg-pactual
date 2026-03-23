# Plano de Trabalho — Desafio Backend BTG Pactual

## Sumário

Este documento descreve o plano de trabalho adotado no desenvolvimento do desafio backend BTG Pactual, organizado no formato de histórias de usuário com estimativas, entregas e status.

O desenvolvimento seguiu a ordem natural de dependência da Clean Architecture: `domain → shared → application → infrastructure → presentation`, garantindo que cada camada fosse compilada e validada antes de ser referenciada pela seguinte.

Durante a execução do item 9 (Teste de Integração), foi identificada uma anomalia de arranque: o container da API tentava consumir a fila do RabbitMQ antes que o serviço de mensageria estivesse pronto para aceitar conexões. Isso gerou o item 10 (Correção de reconexão resiliente), que implementou uma estratégia de retry com backoff exponencial no consumer. Após a correção, o item 11 repetiu o ciclo de testes para validar o comportamento corrigido. Os três itens formam um ciclo intencional de **detectar → corrigir → validar**, comum em fluxos ágeis.

---

## Tarefas

---

### 1. Preparar ambiente de desenvolvimento
> Como desenvolvedor, preciso de um ambiente de desenvolvimento com ferramentas e recursos para que eu possa criar o projeto conforme os requisitos do desafio.

- [x] **Estimativa:** 6h
- [x] **Esperado:** ADR documentando as decisões de ambiente e devcontainer funcional
- [x] **Entregue:** [ADR-0001 devcontainer](../adr/0001-devcontainer.md) e [ADR-0002 Linux-first](../adr/0002-linux-first.md); `docker-compose.yml` do devcontainer disponibilizando instâncias de MongoDB e RabbitMQ; ambiente de desenvolvimento funcional via Dev Containers
- [x] **Status:** ✅ Concluído

---

### 2. Definição de arquitetura
> Como desenvolvedor, preciso de uma definição de arquitetura de projeto para que eu possa criar o roteiro de desenvolvimento da aplicação.

- [x] **Estimativa:** 30min
- [x] **Esperado:** ADR justificando a escolha arquitetural
- [x] **Entregue:** [ADR-0003 Clean Architecture](../adr/0003-escolha-do-clean-archtecture.md) justificando a adoção de Clean Architecture em monorepo NX
- [x] **Status:** ✅ Concluído

---

### 3. Desenvolvimento da camada Domain
> Como desenvolvedor, preciso da camada de domínio implementada para que as regras de negócio e contratos estejam definidos antes das demais camadas.

- [x] **Estimativa:** 1h
- [x] **Esperado:** projeto `libs/domain.csproj` criado e compilado
- [x] **Entregue:** projeto criado e compilado com sucesso, contendo entidades de domínio (`Order`, `OrderItem`), interfaces (`IOrderRepository`) e enumerações (`OrderStatus`), conforme os requisitos do desafio
- [x] **Status:** ✅ Concluído

---

### 4. Desenvolvimento da camada Shared
> Como desenvolvedor, preciso da camada compartilhada implementada para que DTOs, responses e extensões de mapeamento estejam disponíveis para as demais camadas.

- [x] **Estimativa:** 1h
- [x] **Esperado:** projeto `libs/shared.csproj` criado e compilado
- [x] **Entregue:** projeto criado e compilado com sucesso, contendo DTOs de entrada (`OrderMessageDto`, `OrderItemDto`), responses de saída (`OrderResponse`, `ClientSummaryResponse`) e extensões de mapeamento (`OrderMappingExtensions`)
- [x] **Status:** ✅ Concluído

---

### 5. Desenvolvimento da camada Application
> Como desenvolvedor, preciso da camada de aplicação implementada para que os casos de uso do sistema estejam definidos e orquestrados de forma independente de infraestrutura.

- [x] **Estimativa:** 1h
- [x] **Esperado:** projeto `libs/application.csproj` criado e compilado
- [x] **Entregue:** projeto criado e compilado com sucesso, contendo os casos de uso `ProcessOrderUseCase`, `GetOrderTotalUseCase` e `GetOrdersByClientUseCase`, além do contrato `IMessageConsumer`
- [x] **Status:** ✅ Concluído

---

### 6. Desenvolvimento da camada Infrastructure
> Como desenvolvedor, preciso da camada de infraestrutura implementada para que a persistência no MongoDB e o consumo do RabbitMQ estejam encapsulados e desacoplados do domínio.

- [x] **Estimativa:** 2h
- [x] **Esperado:** projeto `libs/infrastructure.csproj` criado e compilado
- [x] **Entregue:** projeto criado e compilado com sucesso, contendo `OrderRepository` (MongoDB), `RabbitMqConsumer` (mensageria), `MongoDbContext`, configurações tipadas e extensão de DI `AddInfrastructure()`; mapeamento do `_id` do MongoDB realizado na camada de infraestrutura via `BsonClassMap`, sem acoplamento ao domínio — ver [ADR-0004 MongoDB](../adr/0004-mongodb.md)
- [x] **Status:** ✅ Concluído

---

### 7. Desenvolvimento da camada Presentation
> Como desenvolvedor, preciso da camada de apresentação implementada para que a API REST e o worker de mensageria estejam disponíveis como pontos de entrada da aplicação.

- [x] **Estimativa:** 2h
- [x] **Esperado:** projeto `apps/orders-api.csproj` criado e compilado
- [x] **Entregue:** projeto criado e compilado com sucesso, contendo `OrderController` com os três endpoints solicitados, `OrderConsumerWorker` como `BackgroundService`, documentação OpenAPI disponível via Scalar em `/scalar`
- [x] **Status:** ✅ Concluído

---

### 8. Criação do docker-compose.yml do ecossistema
> Como desenvolvedor, preciso de um docker-compose.yml na raiz do projeto para que o ecossistema completo da aplicação possa ser levantado com um único comando.

- [x] **Estimativa:** 1h
- [x] **Esperado:** `docker-compose.yml` funcional
- [x] **Entregue:** `docker-compose.yml` com os serviços `orders-api`, `mongodb` e `rabbitmq`; containers nomeados, volume nomeado para o MongoDB (`mongodb-data`), rede privada nomeada (`app-network`) e healthcheck configurado nos serviços de infraestrutura com `condition: service_healthy`
- [x] **Status:** ✅ Concluído

---

### 9. Teste de integração — ciclo 1
> Como desenvolvedor, preciso validar o ecossistema completo para garantir que todos os componentes se comunicam corretamente em ambiente containerizado.

- [x] **Estimativa:** 2h
- [x] **Esperado:** documento com evidências de testes
- [x] **Entregue:** testes executados com os seguintes resultados:
  - ✅ Publicação de mensagem via RabbitMQ Management
  - ✅ Persistência verificada no MongoDB via mongosh
  - ✅ Documentação Scalar acessível em `/scalar`
  - ❌ **Anomalia detectada:** container da API quebrava na inicialização por tentar consumir a fila do RabbitMQ antes do serviço estar pronto para aceitar conexões AMQP — gerou o item 10
- [x] **Status:** ✅ Concluído (com anomalia registrada)

---

### 10. Correção: reconexão resiliente no consumer RabbitMQ
> Como desenvolvedor, preciso que o consumer RabbitMQ não quebre o container na inicialização para que o sistema seja resiliente a condições de race condition entre serviços no arranque do ecossistema.

> 📎 *Originado da anomalia detectada no item 9.*

- [x] **Estimativa:** 2h
- [x] **Esperado:** microserviço não quebra na inicialização e realiza até 10 tentativas de reconexão
- [x] **Entregue:** estratégia de retry com backoff exponencial implementada em `RabbitMqConsumer.cs` — 10 tentativas com delay inicial de 2 segundos, dobrando a cada tentativa (2s → 4s → 8s → ... → 1024s); evidência de funcionamento registrada nos logs do `docker compose up --build`
- [x] **ADR:** não gerou ADR por ser uma decisão de implementação, não de arquitetura
- [x] **Status:** ✅ Concluído

---

### 11. Teste de integração — ciclo 2
> Como desenvolvedor, preciso revalidar o ecossistema após a correção do item 10 para garantir que a anomalia foi resolvida e que todos os fluxos continuam funcionando corretamente.

> 📎 *Repetição do item 9 após correção aplicada no item 10.*

- [x] **Estimativa:** 1h
- [x] **Esperado:** documento com evidências de testes sem anomalias
- [x] **Entregue:** todos os testes executados com sucesso:
  - ✅ Publicação de mensagem via RabbitMQ Management
  - ✅ Persistência verificada no MongoDB via mongosh
  - ✅ Documentação Scalar acessível em `/scalar`
  - ✅ `GET /api/orders/{codigoPedido}/total` — 200 OK com payload correto
  - ✅ `GET /api/orders/clients/{codigoCliente}/count` — 200 OK com contagem correta
  - ✅ `GET /api/orders/clients/{codigoCliente}` — 200 OK com lista de pedidos
  - ✅ Consumer reconectou com sucesso após tentativas de retry (log evidenciado)
- [x] **Status:** ✅ Concluído
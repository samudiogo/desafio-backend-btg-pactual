# Relatório Técnico do Desafio BTG Pactual

## 1. Plano de Trabalho (previsto vs realizado)

### 1.1 Previsto
- Preparar ambiente de desenvolvimento .devcontainer + Docker Compose (MongoDB + RabbitMQ + API)
- Definição de arquitetura: Clean Architecture em monorepo NX (.NET)
- Implementação da camada Domain, Shared, Infrastructure, Application, Presentation
- Consumo de RabbitMQ e persistência em MongoDB
- API com endpoints solicitados
- Testes de integração e qualidade
- Documentação no GitHub (readme + ADRs + diagramas)

### 1.2 Realizado
- Realizado todo o workflow acima com as seguintes entregas:
    - ADR 0001: escolha do .devcontainer (WSL Linux)
    - ADR 0002: Linux-first / Docker Desktop → Docker Linux (WSL)
    - ADR 0003: escolha do Clean Architecture e monorepo NX
    - ADR 0004: mapeamento MongoDB _id na camada de infraestrutura
    - Plano de trabalho (docs/reports/plano-de-trabalho.md)
    - Modelagem de banco (docs/architecture/modelagem-db.md)
    - App em .NET (apps/orders-api + libs/*)
    - docker-compose com MongoDB, RabbitMQ e orders-api
    - testes de integração com cenário real e fallback de retry

### 1.3 Desvios e explicações
- Inclusão adicional de task de hardening: reconexão controlada RabbitMQ (10 tentativas com backoff) por falha observada no arranque inicial. Esse ajustamento fez o sistema ficar mais robusto e atendeu estética da API sem interrupção.
- Documentado no plano de trabalho _in loco_ (item 10 e 11).

### 1.4 Conformidade com plano
- Plano seguindo sem desvios críticos de escopo; ajustes de implementação para estabilização do consumidor são natural refinamento de produto.

## 2. Tecnologias utilizadas
- Linguagem: C# (.NET 10, conforme .csproj do projeto)
- Framework: .NET 10 / ASP.NET Core Web API, Worker Service
- Monorepo: NX + @nx/dotnet
- Banco: MongoDB
- Mensageria: RabbitMQ
- Container: Docker + Docker Compose
- IDE sugerida: Visual Studio Code (Dev Containers) / Visual Studio
- Sistema operacional de desenvolvimento: WSL2 Ubuntu (Linux-first)

## 3. Linguagens, Versões, IDEs, SOs
- C# 12 / .NET 10
- SDK .NET 8.x
- VS Code (versão 1.112) com extensão Remote - Containers
- Visual Studio 2026 Community Edition (opcional)
- WSL2 (Ubuntu 22.04 ou similar)
- Docker Engine 24.x, Docker Compose 2.x

## 4. Diagrama de arquitetura
- Placeholder: `images/diag-arquitetura.png`
- Descrição: WebAPI + Worker + RabbitMQ + MongoDB com camadas de dependência Clean Architecture (Apresentação -> Application -> Domain -> Infrastructure)

## 5. Modelagem da base de dados
- Tecnologia: MongoDB
- Coleção: `orders`
- Documento exemplar:

```json
{
  "_id": "ObjectId",
  "codigoPedido": 1001,
  "codigoCliente": 1,
  "itens": [
    { "produto": "lápis", "quantidade": 100, "preco": 1.10, "valorTotal": 110.00 },
    { "produto": "caderno", "quantidade": 10, "preco": 1.00, "valorTotal": 10.00 }
  ],
  "valorTotal": 120.00,
  "criadoEm": "2026-03-22T00:00:00Z"
}
```
- Repositório `OrderRepository` mapeia `Order` via `MongoDbContext` e filtros por `codigoCliente` / `codigoPedido`.
- Index sugeridos: `codigoPedido` (único), `codigoCliente` (consulta por cliente).

## 6. Diagrama de implantação da solução
- Placeholder: `images/diag-implantacao.png`
- Informar containers: `mongo`, `rabbitmq`, `orders-api`.
- Rede: bridge interna do Docker Compose.

## 7. Diagrama de infra com recursos de cloud
- Placeholder: `images/diag-infra-cloud.png`
- Exemplo (se não usar nuvem, descrever VM local, WSL2 e container host local).

## 8. Evidência de testes funcionais da aplicação
- Teste de fluxo básico realizado:
  1. Start do `docker-compose up --build` contendo MongoDB, RabbitMQ, orders-api.
  2. Publicação de mensagem via RabbitMQ Management ou script de publicação com payload JSON de pedido.
  3. Verificação de persistência no MongoDB (`orders` collection).
  4. API endpoints verificados:
     - `GET /api/orders/{codigoPedido}/total`
     - `GET /api/orders/clients/{codigoCliente}/count`
     - `GET /api/orders/clients/{codigoCliente}`
  5. Home do Scalar disponível e funcional.
- Logs de teste (placeholder): `logs/testes-funcionais.log`

## 9. Publicação de código e imagens
- GitHub: `https://github.com/samudiogo/desafio-backend-btg-pactual`
- DockerHub: `https://hub.docker.com/repositories/samudiogo/desafio-backend-btg-pactual`

## 10. Referências utilizadas
- Documento do desafio BTG Pactual (README)
- Documentação oficial .NET e ASP.NET Core
- Documentação MongoDB .NET Driver
- Documentação RabbitMQ e RabbitMQ.Client
- Documentação NX e @nx/dotnet

## 11. Outros itens relevantes
- Arquitetura baseada em Clean Architecture e DDD light.
- Reuso de camadas por projetos (domain, shared, infrastructure, application, presentation).
- Uso de ADRs para decisões de arquitetura e ambiente.
- Estratégia de reconexão resiliente no RabbitMQ consumer com exponential backoff (até 10 tentativas).
- Uso de ADR para garantia de domínio não acoplado ao driver de persistência (por exemplo, `_id` mapeado na infra).

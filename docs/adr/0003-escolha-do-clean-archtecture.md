# ADR 0003: Arquitetura monorepos com Clean Architecture

Status: Aceito
Data: 2026-03-22

## Contexto
- Aumentar a familiaridade com o NX monorepos.
- Adoção de Clean Architecture visando boas práticas de desenvolvimento.

## Problema
- Como estruturar o código para manter separação de responsabilidades, testabilidade e escalabilidade em um projeto .NET multiplataforma?
- Atender aos requisitos da vaga, como experiência com arquitetura mono repositório, sólidos conhecimentos de Clean Architecture, Clean Code e princípios SOLID.

## Decisão
1. Criação do workspace usando NX `create-nx-workspace`.
2. Instalação do plugin `@nx/dotnet`.
3. Definição da pasta "apps/" como camada de presentation com o projeto C# WebAPI.
4. Definição da pasta "lib/" para armazenar as camadas de `application`, `domain`, `infrastructure` e `shared`.
5. Criação de .slnx na raiz do workspace.

## Justificativa
- **Atende os requisitos da vaga**: Apesar de indicar uma tendência de `over engineering`, o propósito da aplicação é justamente evidenciar capacidade técnica para construção de sistemas complexos e escaláveis.
- **NX monorepo**: A configuração de monorepo com NX é bem documentada, simples, alinhada com experiência técnica em JS/TypeScript e arquivos JSON, e facilita integração com CI usando GitHub Actions e NX Cloud. Permite builds incrementais mais rápidos e compartilhamento de código entre projetos.
- **Clean Architecture**: Separação das responsabilidades por camada, abordagem domain-first, desacoplamento coeso da infraestrutura com o domínio, adoção de use cases para não inflar uma classe de serviços com múltiplos métodos. Facilita testes unitários/integração e permite deploy independente de módulos.
- **Clean Code e SOLID**: Criação de códigos seguindo boas práticas, facilitando integrações com testes e com a camada de apresentação. Melhora DX com NX caching e documentação automática via OpenAPI.

## Consequências
- **Aumento da complexidade**: Se considerar o requisito do desafio, poderia alcançar o objetivo fazendo um simples monolito, ou até mesmo projeto Clean Architecture abdicando da camada monorepo. Porém descartei pois a minha intenção foi mostrar conhecimento técnico para trabalhar com monorepos e projetos de arquiteturas complexas como Clean Architecture e DDD.
- **GIT Monorepo**: Ao invés de criar múltiplos repositórios git, adotei usar um único repositório para versionar a API, libs e oferecendo no futuro a possibilidade de um projeto frontend, no mesmo repositório.
- **Orquestração sincronizada**: Ao fazer a compilação com NX, aplicação atualiza e faz o build apenas nos projetos modificados e que de alguma forma estão interconectados.
- **Projeto pronto para testes**: Por ter usado boas práticas, a aplicação está apta para receber projetos de testes unitários. Como a aplicação `apps/orders-api` tem OpenAPI e Scalar, a documentação de uso de API foi natural.
- **Benefícios positivos**: Builds incrementais mais rápidos, compartilhamento de código entre projetos, documentação automática via OpenAPI.

## Alternativas consideradas
- **Monolito** — Rejeitado: Alto nível de acoplamento, não demonstra a maturidade técnica explícita nos requisitos da vaga.
- **Projeto sem monorepo** — Rejeitado: Teria que criar múltiplos repositórios (pelo menos dois: presentation/frontend e backend).
- **DDD N-Layers** — Rejeitado: A abordagem do Clean Architecture é versão mais enxuta do DDD.
- **Hexagonal** — Rejeitado: Similar ao Clean Architecture mas mais complexo para este escopo, adicionando overhead desnecessário.
- **AWS Serverless** — Rejeitado: Apesar de atender parcialmente aos requisitos técnicos do desafio usando AWS Lambda, SQS, Api Gateway e DynamoDB, o desafio sairia de custo Zero para ~90 reais.
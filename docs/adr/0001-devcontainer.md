# ADR 0001: Escolha de ambiente de desenvolvimento com .devcontainer (WSL Linux)

Status: Aceito
Data: 2026-03-22

## Contexto
- O repositório é um projeto .NET multiplataforma com dependências locais e infraestrutura de container.
- Há necessidade de um ambiente de desenvolvimento padronizado entre a equipe, com setup reproduzível para Linux/WSL.

## Problema
- Desenvolvedores em diferentes máquinas têm configurações distintas de IDE, SDKs, ferramentas e versões.
- O setup manual é propenso a divergências e falhas ocasionais em execuções de CI/local.

## Decisão
- Adotar `.devcontainer` como abordagem oficial de ambiente de desenvolvimento.
- Executar o container de desenvolvimento dentro de uma instância Linux (WSL) para garantir compatibilidade com a infraestrutura e ferramentas de container.
- Incluir Dockerfile e `devcontainer.json` no repositório para setup automático em VS Code (Dev Containers).

## Justificativa
- `.devcontainer` fornece ambiente consistente, idempotente e versionado junto com o código.
- WSL Linux é comum em máquinas de desenvolvedores Windows; mantém paridade com ambientes de produção Linux.
- Facilita onboarding rápido de novos membros e reduz problemas de “funciona na minha máquina”.

## Consequências
- Força o uso de Docker/VS Code Remote - Containers ou VS Code Dev Containers para desenvolvimento ideal.
- Menos dependência de configurações locais manuais de SDKs e ferramentas.
- Exige manutenção do arquivo de container em paralelo às mudanças de dependências do projeto.

## Alternativas consideradas
- Setup manual local (rejeitado por pouca consistência)
- Scripts de bootstrap (imprecisos com múltiplas versões de sistema operativo)
- Uso exclusivo de máquinas virtuais pesadas (maior custo e complexidade)

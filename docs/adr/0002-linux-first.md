# ADR 0002: Linux como Source of Truth no WSL para desenvolvimento

Status: Aceito
Data: 2026-03-22

## Contexto
- O projeto adota `.devcontainer` para ambiente de desenvolvimento padronizado (ADR 0001).
- Desenvolvedores usam Windows com WSL2 como camada de abstração para ambiente Linux.
- Há necessidade de coesão entre filesystem, gerenciamento de SSH Keys e runtime de containers.

## Problema
- Ambiente híbrido (Windows + WSL + Docker Desktop) gera inconsistências:
  - Paths inconsistentes: `/mnt/e/...` versus `/home/...` causam lentidão e bugs em mount volumes
  - Múltiplas fontes de SSH Agent (Windows, WSL, Docker Desktop) geram falhas intermitentes
  - Docker Desktop não acessa nativamente SSH Agent do WSL, quebrando fluxo de `git push` dentro de containers
- Falta de fonte única de verdade invalida princípios de ambiente reproduzível

## Decisão
1. Estabelecer WSL Linux como única "source of truth" para desenvolvimento.
2. Desinstalar Docker Desktop Windows; instalar Docker nativo na instância WSL (Ubuntu).
3. Manter SSH Keys e SSH Agent exclusivamente no WSL Linux.
4. Configurar mounts de volumes do devcontainer usando paths nativos do Linux (`/home/...`).

## Justificativa
- **Coesão de ambiente**: Sai de modelo híbrido (Windows + Linux) para modelo nativo Linux, removendo camada de tradução.
- **SSH nativo**: SSH Agent no Linux é acessível nativamente pelo Docker Linux, resolvendo problema raiz de comunicação com GitHub.
- **Performance**: Eliminação de conversão de paths (`/mnt/e` → `/home`) melhora velocidade de I/O em volumes.
- **Segurança**: Uma única fonte de identidade (SSH Keys no WSL) reduz superfície de ataque.
- **Reprodutibilidade**: Alinha com objetivo de environment-as-code do `.devcontainer`.

## Consequências
- **Perda de conveniência**: Docker Desktop GUI desaparece; monitoramento via CLI (`docker logs`, `docker ps`, `docker exec`).
- **Curva de aprendizado**: Exige mais maturidade técnica da equipe para debugging de containers.
- **Dependência de WSL**: Desempenho agora depende diretamente de WSL2 performance (mitigation: usar WSL2 distro nativa com bom espaço em disco).
- **Janela de transição**: Migração exige reconfiguração de `.devcontainer.json` e workflows locais.

## Alternativas consideradas
- **Manter Docker Desktop + SSH forwarding** — Rejeitado: SSH agent forwarding complexo, falhas intermitentes mesmo com configuração.
- **VS Code Remote SSH** — Rejeitado: funcionou inicialmente mas perdeu consistência; requer SSH server extra em WSL.
- **socat TCP bridge entre Windows e WSL** — Rejeitado: incompatível com ssh-agent protocol.
- **Cloud-based development (GitHub Codespaces)** — Rejeitado: custos elevados para uso de simples ambiente de aprendizado individual.
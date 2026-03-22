1. Preparar ambiente de desenvolvimento 
    - Como desenvolvedor preciso de um ambiente de desenvolvimento com ferramentas e recursos
     para que eu possa  criar o projeto conforme os requisitos da vaga e do desafio
    - Estimativa: 6h
    - Esperado: ADR, ambiente de desenvolvimento funcional
    - Entregue: ADR (0001 e 0002), docker-compose.yml disponibilizadno devcontainer, instancia de mongodb e rabbitmq
    - Status: Concluido

2. Definição de arquitetura
    - Como desenvolvedor preciso de uma definição de arquitetura de projeto e arquivos para que eu possa criar o roteiro da criação da aplicação.
    - Estimativa: 30min
    - Esperado: ADR
    - Entregue: ADR (0003-escolha-do-clean-archtecture.md) justificando a adoção de "Clean Archtecture"
    - Status: Concluido

3. Desenvolvimento da camada Domain
    - Criação do projeto tipo C# classlib `libs/domain.csproj` contendo as regras de negócios extraidas do requisto do desafio, bem como as interfaces e contratos
    - Estimativa: 1h
    - Esperado: projeto criado
    - Entregue: projeto criado e compilado com sucesso, contendo domain entities, interfaces e enum, conforme o requisto do desafio.
    - Status: Concluido

4. Desenvolvimento da camada shared
    - Criação do projeto tipo C# classlib `libs/shared.csproj` contendo os recursos que serão compartilhados entre as camadas, como DTOs, Extensions e Responses
    - Estimativa: 1h
    - Esperado: projeto criado
    - Entregue: projeto criado e compilado com sucesso.
    - Status: Concluido

5. Desenvolvimento da camada infrastructure
    - Criação do projeto tipo C# classlib `libs/infrastructure.csproj` contendo as implementações do consumer de mensageria, injeção de dependencia e persistência com MongoDB com repository pattern
    - Estimativa: 2h
    - Esperado: projeto criado
    - Entregue: projeto criado e compilado com sucesso.
    - Status: Concluido

6. Desenvolvimento da camada application
    - Criação do projeto tipo C# classlib `libs/application.csproj` contendo os useCases e ports necessários como IMessageConsumer
    - Estimativa: 1h
    - Esperado: projeto criado
    - Entregue: projeto criado e compilado com sucesso, contendo domain entities, interfaces e enum, conforme o requisto do desafio.
    - Status: Concluido

7. Desenvolvimento da camada presentation
    - Criação do projeto tipo C# WEBAPI `apps/orders-api.csproj` contendo um worker para processar as filas e uma controller para retornar os dados de pedidos salvos no banco de dados.
    - Estimativa: 2h
    - Esperado: projeto criado
    - Entregue: projeto criado e compilado com sucesso, contendo controller, worker e documentação com OpenAPI e Scalar.
    - Status: Concluido

8. Criação de docker-compose.yml do ecosistema
    - Criação do docker-compose.yml na raiz do workspace contendo os serviços de mongo, rabbitmq e a api, use conteiners nomeado, volume nomeado para o mongo e rede privada nomeada.
    - Estimativa: 1h
    - Esperado: docker-compose.yml
    - Entregue: docker-compose.yml
    - Status: Concluido


9. Teste de integração
    - Levantar o ecosistema escrito no docker-compose.yml na raiz do projeto e fazer os seguintes testes:
        - publicação de mensagem no RabbitMQ Management
        - conferir registro salvo no mongodb via mongoose ou mongodb compass
        - conferir documentação de api pelo url /scalar do microservico orders-api
        - executar endpoints:
            - /api/orders/1001/total
            - /api/orders/clients/1/count
            - /api/orders/clients/1

    - Estimativa: 2h
    - Esperado: Documento com evidências de testes.
    - Entregue: projeto projeto testado com sucesso, todavia ao levar o sistema, notei que o container do microserviço estava quebrando pois estava tentando consumir a fila do rabbitmq antes do servço de mensageria está pronto pra uso. Criei uma nova demanda para correção do bug.
    - Status: Concluido

10. Correção que quebra de API por não não erro de conexão com serviço de mensageria
    - Adicionar no consumer um abordagem de looping controlado com 10 tentativas de conexão ao serviço de mensageria do RabbitMQ, a cada tentativa sem sucesso, incremente um delay exponecial iniciando em 2 segundos.

    - Estimativa: 2h
    - Esperado: microserviço não quebra e sempre tenta reconectar durante até 10 tentativas.
    - Entregue: Adicionado a melhoria no arquivo RabbitMqConsumer.cs e evidencia de teste (log do docker compose up --build).
    - Status: Concluido


11.  Refazer o Teste de integração
    - Levantar o ecosistema escrito no docker-compose.yml na raiz do projeto e fazer os seguintes testes:
        - publicação de mensagem no RabbitMQ Management
        - conferir registro salvo no mongodb via mongoose ou mongodb compass
        - conferir documentação de api pelo url /scalar do microservico orders-api
        - executar endpoints:
            - /api/orders/1001/total
            - /api/orders/clients/1/count
            - /api/orders/clients/1

    - Estimativa: 2h
    - Esperado: Documento com evidências de testes.
    - Entregue: projeto projeto testado com sucesso e o documento com as evidências de testes.
    - Status: Concluido




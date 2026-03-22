## Coleção orders (MongoDB)

- `_id`: ObjectId
- `codigoPedido`: int (único)
- `codigoCliente`: int
- `itens`: array de objetos
  - `produto`: string
  - `quantidade`: int
  - `preco`: decimal
  - `valorTotal`: decimal
- `valorTotal`: decimal (calculado no domínio)
- `criadoEm`: DateTime (opcional)

### Estratégia de mapeamento

- Domain entity: `BtgPactual.Domain.Entities.Order`
- Infra mapping: `MongoDbContext` + `OrderRepository`
- `SetIgnoreExtraElements(true)` para evitar depender de campos extras
- Domínio permanece sem dependência do driver MongoDB
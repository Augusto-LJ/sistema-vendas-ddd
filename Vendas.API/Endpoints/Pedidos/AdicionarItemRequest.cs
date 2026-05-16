namespace Vendas.API.Endpoints.Pedidos;

public record AdicionarItemRequest(Guid ProdutoId, int Quantidade);
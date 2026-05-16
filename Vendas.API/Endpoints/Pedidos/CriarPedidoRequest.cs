namespace Vendas.API.Endpoints.Pedidos;

public record CriarPedidoRequest(Guid ClienteId, Guid EnderecoId);
namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEntregue;
public sealed class MarcarPedidoComoEntregueResultDto
{
    public Guid PedidoId { get; init; }
    public string Status { get; init; } = string.Empty;
}

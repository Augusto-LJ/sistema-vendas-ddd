namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEntregue;
public sealed class MarcarPedidoComoEntregueCommand(Guid pedidoId)
{
    public Guid PedidoId { get; } = pedidoId;
}

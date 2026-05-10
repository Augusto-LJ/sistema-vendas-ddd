namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;
public sealed class MarcarPedidoComoEntregueCommand(Guid pedidoId)
{
    public Guid PedidoId { get; } = pedidoId;
}

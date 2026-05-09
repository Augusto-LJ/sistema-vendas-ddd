namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEnviado;
public sealed class MarcarPedidoComoEnviadoCommand(Guid pedidoId)
{
    public Guid PedidoId { get; } = pedidoId;
}

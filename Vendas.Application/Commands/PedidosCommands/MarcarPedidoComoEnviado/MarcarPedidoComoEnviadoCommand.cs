namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEnviado;
public sealed class MarcarPedidoComoEnviadoCommand(Guid pedidoId)
{
    public Guid PedidoId { get; } = pedidoId;
}

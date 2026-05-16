namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEmSeparacao;
public sealed class MarcarPedidoComoEmSeparacaoCommand(Guid pedidoId)
{
    public Guid PedidoId { get; } = pedidoId;
}

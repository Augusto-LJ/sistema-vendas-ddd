namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEmSeparacao;
public sealed class MarcarPedidoComoEmSeparacaoResultDto
{
    public Guid PedidoId { get; init; }
    public string StatusPedido { get; init; } = string.Empty;
}

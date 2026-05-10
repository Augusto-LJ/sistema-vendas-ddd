namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEntregue;
public sealed class MarcarPedidoComoEntregueResultDto
{
    public Guid PedidoId { get; init; }
    public string Status { get; init; } = string.Empty;
}

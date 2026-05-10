namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEnviado;
public sealed class MarcarPedidoComoEnviadoResultDto
{
    public Guid PedidoId { get; init; }
    public string Status { get; init; } = string.Empty;
}

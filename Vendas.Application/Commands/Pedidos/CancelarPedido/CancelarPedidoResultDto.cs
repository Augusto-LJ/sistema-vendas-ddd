namespace Vendas.Application.Commands.Pedidos.CancelarPedido;
public sealed class CancelarPedidoResultDto
{
    public Guid PedidoId { get; init; }
    public string Status { get; init; } = string.Empty;
}

namespace Vendas.Application.Commands.Pedidos.RemoverItemDoPedido;
public sealed class RemoverItemDoPedidoResultDto(Guid pedidoId, decimal valorTotal, string statusPedido)
{
    public Guid PedidoId { get; } = pedidoId;
    public decimal ValorTotal { get; } = valorTotal;
    public string StatusPedido { get; } = statusPedido;
}
namespace Vendas.Application.Commands.Pedidos.RemoverItemDoPedido;
public sealed class RemoverItemDoPedidoCommand(Guid pedidoId, Guid itemId)
{
    public Guid PedidoId { get; } = pedidoId;
    public Guid ItemId { get; } = itemId;
}

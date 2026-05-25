namespace Vendas.Application.Queries.Pedidos.ObterPedidoCompletoPorId;
public sealed class ObterPedidoCompletoPorIdQuery
{
    public Guid PedidoId { get; }

    public ObterPedidoCompletoPorIdQuery(Guid pedidoId)
    {
        if (pedidoId == Guid.Empty)
            throw new ArgumentException("O ID do pedido não pode ser vazio.", nameof(pedidoId));

        PedidoId = pedidoId;
    }
}

namespace Vendas.Application.Commands.PedidosCommands.AdicionarItemAoPedido;
public sealed class AdicionarItemAoPedidoCommand(Guid pedidoId, Guid produtoId, int quantidade)
{
    public Guid PedidoId { get; } = pedidoId;
    public Guid ProdutoId { get; } = produtoId;
    public int Quantidade { get; } = quantidade;
}

namespace Vendas.Application.Commands.PedidosCommands.AdicionarItemAoPedido;
public sealed class AdicionarItemAoPedidoCommand(Guid pedidoId, Guid produtoId, string nomeProduto, decimal precoUnitario, int quantidade)
{
    public Guid PedidoId { get; } = pedidoId;
    public Guid ProdutoId { get; } = produtoId;
    public string NomeProduto { get; } = nomeProduto;
    public decimal PrecoUnitario { get; } = precoUnitario;
    public int Quantidade { get; } = quantidade;
}

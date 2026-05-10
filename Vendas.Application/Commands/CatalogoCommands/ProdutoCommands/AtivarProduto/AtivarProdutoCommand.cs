namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;
public sealed class AtivarProdutoCommand(Guid produtoId)
{
    public Guid ProdutoId { get; init; } = produtoId;
}

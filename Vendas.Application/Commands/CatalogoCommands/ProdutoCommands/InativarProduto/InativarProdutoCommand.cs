namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.InativarProduto;
public sealed class InativarProdutoCommand(Guid produtoId)
{
    public Guid ProdutoId { get; } = produtoId;
}

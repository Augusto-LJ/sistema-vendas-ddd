namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarCategoriaDoProduto;
public sealed class AlterarCategoriaDoProdutoCommand(Guid produtoId, Guid novaCategoriaId)
{
    public Guid ProdutoId { get; } = produtoId;
    public Guid NovaCategoriaId { get; } = novaCategoriaId;
}

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.RenomearCategoria;
public sealed class RenomearCategoriaCommand(Guid categoriaId, string novoNome)
{
    public Guid CategoriaId { get; init; } = categoriaId;
    public string NovoNome { get; init; } = novoNome;
}

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.AtivarCategoria;
public sealed class AtivarCategoriaCommand(Guid categoriaId)
{
    public Guid CategoriaId { get; init; } = categoriaId;
}

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.InativarCategoria;
public sealed class InativarCategoriaCommand(Guid categoriaId)
{
    public Guid CategoriaId { get; init; } = categoriaId;
}

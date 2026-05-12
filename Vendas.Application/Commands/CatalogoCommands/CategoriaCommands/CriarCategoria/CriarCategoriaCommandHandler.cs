using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.CriarCategoria;
public sealed class CriarCategoriaCommandHandler(ICategoriaRepository categoriaId)
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaId;

    public async Task<CriarCategoriaResultDto> HandleAsync(CriarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = new Categoria(command.Nome, command.Descricao);

        await _categoriaRepository.AdicionarAsync(categoria, cancellationToken);

        return new CriarCategoriaResultDto
        {
            CategoriaId = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao,
            Ativa = categoria.Ativa
        };
    }
}

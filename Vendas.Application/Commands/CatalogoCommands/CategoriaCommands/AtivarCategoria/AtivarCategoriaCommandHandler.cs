using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.AtivarCategoria;
public sealed class AtivarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<AtivarCategoriaResultDto> HandleAsync(AtivarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.CategoriaId, cancellationToken);

        if (categoria is null)
            throw new DomainException("Categoria não encontrada.");

        categoria.Ativar();

        await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);

        return new AtivarCategoriaResultDto
        {
            CategoriaId = categoria.Id
        };
    }
}

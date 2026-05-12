using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.InativarCategoria;
public sealed class InativarCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<InativarCategoriaResultDto> HandleAsync(InativarCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.CategoriaId, cancellationToken);

        if (categoria is null)
            throw new DomainException("Categoria não encontrada.");

        categoria.Inativar();

        await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);

        return new InativarCategoriaResultDto
        {
            CategoriaId = categoria.Id
        };
    }
}

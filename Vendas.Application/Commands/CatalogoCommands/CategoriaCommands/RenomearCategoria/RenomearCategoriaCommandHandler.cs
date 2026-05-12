using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.RenomearCategoria;
public sealed class RenomearCategoriaCommandHandler(ICategoriaRepository categoriaRepository)
{
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<RenomearCategoriaResultDto> HandleAsync(RenomearCategoriaCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.CategoriaId, cancellationToken);

        if (categoria is null)
            throw new DomainException("Categoria não encontrada.");

        categoria.AlterarNome(command.NovoNome);

        await _categoriaRepository.AtualizarAsync(categoria, cancellationToken);

        return new RenomearCategoriaResultDto
        {
            CategoriaId = categoria.Id,
            NovoNome = categoria.Nome
        };
    }
}

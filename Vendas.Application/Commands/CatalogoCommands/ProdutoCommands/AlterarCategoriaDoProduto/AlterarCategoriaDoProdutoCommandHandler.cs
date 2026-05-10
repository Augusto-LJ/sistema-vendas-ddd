using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarCategoriaDoProduto;
public sealed class AlterarCategoriaDoProdutoCommandHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<AlterarCategoriaDoProdutoResultDto> HandleAsync(AlterarCategoriaDoProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.NovaCategoriaId, cancellationToken);

        if (categoria is null)
            throw new DomainException("Categoria não encontrada.");

        Guard.Against<DomainException>(!categoria.Ativa, "Não é possível associar um produto a uma categoria inativa");

        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        produto.AlterarCategoria(categoria.Id);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AlterarCategoriaDoProdutoResultDto
        {
            ProdutoId = produto.Id,
            CategoriaId = produto.CategoriaId,
        };
    }
}

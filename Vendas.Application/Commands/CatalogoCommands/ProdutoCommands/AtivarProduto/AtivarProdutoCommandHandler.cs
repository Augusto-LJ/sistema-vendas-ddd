using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtivarProduto;
public sealed class AtivarProdutoCommandHandler(IProdutoRepository produtoRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task<AtivarProdutoResultDto> HandleAsync(AtivarProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        produto.Ativar();

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AtivarProdutoResultDto
        {
            ProdutoId = produto.Id
        };
    }
}

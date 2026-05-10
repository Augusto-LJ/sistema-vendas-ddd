using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AjustarEstoque;
public sealed class AjustarEstoqueCommandHandler(IProdutoRepository produtoRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task<AjustarEstoqueResultDto> HandleAsync(AjustarEstoqueCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        produto.AjustarEstoque(command.NovoEstoque, command.Motivo);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AjustarEstoqueResultDto
        {
            ProdutoId = produto.Id,
            NovoEstoque = produto.Estoque,
            Motivo = command.Motivo
        };
    }
}

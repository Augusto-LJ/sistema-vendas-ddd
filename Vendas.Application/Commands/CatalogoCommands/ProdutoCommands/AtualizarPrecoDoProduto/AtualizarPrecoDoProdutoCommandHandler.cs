using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizarPrecoDoProduto;
public sealed class AtualizarPrecoDoProdutoCommandHandler(IProdutoRepository produtoRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task<AtualizarPrecoDoProdutoResultDto> HandleAsync(AtualizarPrecoDoProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        var novoPreco = new PrecoProduto(command.NovoPreco);

        produto.AlterarPreco(novoPreco);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);
        return new AtualizarPrecoDoProdutoResultDto
        {
            ProdutoId = produto.Id,
            NovoPreco = produto.Preco.Valor,
        };
    }
}

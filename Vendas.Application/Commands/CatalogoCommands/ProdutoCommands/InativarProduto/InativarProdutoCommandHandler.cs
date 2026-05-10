using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.InativarProduto;
public sealed class InativarProdutoCommandHandler(IProdutoRepository produtoRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task<InativarProdutoResultDto> HandleAsync(InativarProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);
    
        if (produto is null)
            throw new DomainException("Produto não encontrado.");
    
        produto.Inativar();
    
        await _produtoRepository.AtualizarAsync(produto, cancellationToken);
    
        return new InativarProdutoResultDto
        {
            ProdutoId = produto.Id
        };
    }
}
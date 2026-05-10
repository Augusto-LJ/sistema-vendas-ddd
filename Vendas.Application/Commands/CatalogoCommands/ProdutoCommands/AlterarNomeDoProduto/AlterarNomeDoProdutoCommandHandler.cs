using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeDoProduto;
public sealed class AlterarNomeDoProdutoCommandHandler(IProdutoRepository produtoRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;

    public async Task<AlterarNomeDoProdutoResultDto> HandleAsync(AlterarNomeDoProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var produto = await _produtoRepository.ObterPorIdAsync(command.ProdutoId, cancellationToken);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        var nome = new NomeProduto(command.NovoNome);

        produto.AlterarNome(nome);

        await _produtoRepository.AtualizarAsync(produto, cancellationToken);

        return new AlterarNomeDoProdutoResultDto
        {
            ProdutoId = produto.Id,
            NovoNome = nome.Valor,
        };
    }
}

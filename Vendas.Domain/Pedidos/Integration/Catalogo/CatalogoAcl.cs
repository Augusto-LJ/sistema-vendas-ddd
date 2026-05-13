using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;
public sealed class CatalogoAcl
{
    private readonly ICatalogoGateway _gateway;

    public CatalogoAcl(ICatalogoGateway gateway)
    {
        Guard.AgainstNull(gateway, nameof(gateway));
        _gateway = gateway;
    }

    public async Task<ProdutoSnapshot> ObterProdutoSnapshotAsync(Guid produtoId, CancellationToken cancellationToken = default)
    {
        var produtoDto = await _gateway.ObterProdutoPorIdAsync(produtoId, cancellationToken);

        if (produtoDto is null)
            throw new DomainException($"Produto não encontrado no catálogo");

        return new ProdutoSnapshot(produtoDto.Id, produtoDto.Nome, produtoDto.Preco);
    }

    public async Task ValidarEstoqueAsync(Guid produtoId, int quantidade, CancellationToken cancellationToken = default)
    {
        var possuiEstoque = await _gateway.PossuiEstoqueDisponivelAsync(produtoId, quantidade, cancellationToken);

        if (!possuiEstoque)
            throw new DomainException($"Estoque insuficiente para o produto");
    }
}
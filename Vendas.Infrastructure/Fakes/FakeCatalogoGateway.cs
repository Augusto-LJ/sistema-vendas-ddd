using Vendas.Domain.Pedidos.Integration.Catalogo;

namespace Vendas.Infrastructure.Fakes;
public sealed class FakeCatalogoGateway : ICatalogoGateway
{
    private static readonly Dictionary<Guid, ProdutoDto> _produtos = new()
    {
        { Guid.Parse("11111111-1111-1111-1111-111111111111"), new ProdutoDto(Guid.Parse("10000000-1000-1111-1111-111111111111"), "Produto 1", 10.0m) },
        { Guid.Parse("22222222-2222-2222-2222-222222222222"), new ProdutoDto(Guid.Parse("20000000-2000-2222-2222-222222222222"), "Produto 2", 20.0m) },
        { Guid.Parse("33333333-3333-3333-3333-333333333333"), new ProdutoDto(Guid.Parse("30000000-3000-3333-3333-333333333333"), "Produto 3", 30.0m) },
        { Guid.Parse("44444444-4444-4444-4444-444444444444"), new ProdutoDto(Guid.Parse("40000000-4000-4444-4444-444444444444"), "Produto 4", 40.0m) }
    };

    public Task<ProdutoDto?> ObterProdutoPorIdAsync(Guid produtoId, CancellationToken cancellationToken = default)
    {
        _produtos.TryGetValue(produtoId, out var produto);
        return Task.FromResult(produto);
    }
}

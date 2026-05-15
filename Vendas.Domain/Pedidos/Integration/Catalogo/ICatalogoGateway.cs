namespace Vendas.Domain.Pedidos.Integration.Catalogo;
public interface ICatalogoGateway
{
    Task<ProdutoDto> ObterProdutoPorIdAsync(Guid produtoId, CancellationToken cancellationToken = default);
}

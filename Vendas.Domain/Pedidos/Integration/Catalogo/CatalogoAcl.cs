namespace Vendas.Domain.Pedidos.Integration.Catalogo;
public sealed class CatalogoAcl
{
    public (string nomeProduto, decimal precoUnitario) TraduzirProduto(ProdutoDto dto)
    {
        return (dto.Nome, dto.Preco);
    }
}
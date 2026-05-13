using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Catalogo;
public sealed class ProdutoSnapshot : ValueObject
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public decimal PrecoUnitario { get; }
    public ProdutoSnapshot(Guid produtoId, string nomeProduto, decimal precoUnitario)
    {
        Guard.AgainstEmptyGuid(produtoId, nameof(produtoId), "ProdutoId inválido");
        Guard.AgainstNullOrWhiteSpace(nomeProduto, nameof(nomeProduto), "Nome do produto é obrigatório");
        Guard.Against<DomainException>(precoUnitario <= 0, "Preço unitário deve ser maior que zero");

        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        PrecoUnitario = precoUnitario;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProdutoId;
        yield return NomeProduto;
        yield return PrecoUnitario;
    }
}

namespace Vendas.Application.Queries.Pedidos.DTOs;
public sealed class ItemResumoDto
{
    public Guid ProdutoId { get; init; }
    public string NomeProduto { get; init; } = string.Empty;
    public decimal PrecoUnitario { get; init; }
    public int Quantidade { get; init; }
    public decimal ValorTotal { get; init; }
}

namespace Vendas.Application.Queries.Pedidos.DTOs;
public sealed class PagamentoPorStatusDto
{
    public Guid PagamentoId { get; init; }
    public string NumeroPedido { get; init; } = string.Empty;
    public Guid ClienteId { get; init; }
    public decimal ValorTotal { get; init; }
    public string StatusPagamento { get; init; } = string.Empty;
    public string MetodoPagamento { get; init; } = string.Empty;
    public string? CodigoTransacao { get; init; }
    public DateTime? DataPagamento { get; init; }
}

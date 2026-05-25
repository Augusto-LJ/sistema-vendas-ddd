namespace Vendas.Application.Queries.Pedidos.DTOs;
public sealed class PedidoCompletoDto
{
    public Guid Id { get; init; }
    public string NumeroPedido { get; init; } = string.Empty;
    public Guid ClienteId { get; init; }
    public decimal ValorTotal { get; init; }
    public string StatusPedido { get; init; } = string.Empty;
    public DateTime DataCriacao { get; init; }
    public DateTime? DataAtualizacao { get; init; }
    public EnderecoEntregaDto Endereco { get; init; } = null!;
    public IReadOnlyList<ItemResumoDto> Itens { get; init; } = [];
    public IReadOnlyList<PagamentoDto> Pagamentos { get; init; } = [];
}

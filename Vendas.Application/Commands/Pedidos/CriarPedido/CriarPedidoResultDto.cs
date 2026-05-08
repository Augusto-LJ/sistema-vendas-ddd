namespace Vendas.Application.Commands.Pedidos.CriarPedido;
public sealed class CriarPedidoResultDto(Guid pedidoId, string numeroPedido, DateTime dataCriacao, decimal valorTotal, string status)
{
    public Guid PedidoId { get; } = pedidoId;
    public string NumeroPedido { get; } = numeroPedido;
    public DateTime DataCriacao { get; } = dataCriacao;
    public decimal ValorTotal { get; } = valorTotal;
    public string Status { get; } = status;
}

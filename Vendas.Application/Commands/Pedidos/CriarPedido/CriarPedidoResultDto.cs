namespace Vendas.Application.Commands.Pedidos.CriarPedido;
public sealed class CriarPedidoResultDto
{
    public Guid PedidoId { get; }
    public string NumeroPedido { get; }
    public DateTime DataCriacao { get; }
    public decimal ValorTotal { get; }
    public string Status { get; }

    public CriarPedidoResultDto(Guid pedidoId, string numeroPedido, DateTime dataCriacao, decimal valorTotal, string status)
    {
        PedidoId = pedidoId;
        NumeroPedido = numeroPedido;
        DataCriacao = dataCriacao;
        ValorTotal = valorTotal;
        Status = status;
    }
}

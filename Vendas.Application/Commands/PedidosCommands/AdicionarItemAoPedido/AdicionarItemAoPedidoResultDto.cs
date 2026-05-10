namespace Vendas.Application.Commands.PedidosCommands.AdicionarItemAoPedido;
public sealed class AdicionarItemAoPedidoResultDto(Guid PedidoId, decimal ValorTotal, string Status)
{
    public Guid PedidoId { get; } = PedidoId;
    public decimal ValorTotal { get; } = ValorTotal;
    public string Status { get; } = Status;
}


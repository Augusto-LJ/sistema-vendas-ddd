namespace Vendas.Application.Commands.Pedidos.CancelarPedido;
public sealed class CancelarPedidoCommand(Guid pedidoId, string codigoMotivo)
{
    public Guid PedidoId { get; } = pedidoId;
    public string CodigoMotivo { get; } = codigoMotivo;
}

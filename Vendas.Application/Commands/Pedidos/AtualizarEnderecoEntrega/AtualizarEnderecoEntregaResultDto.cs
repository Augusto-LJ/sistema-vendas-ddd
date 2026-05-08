namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;
public sealed class AtualizarEnderecoEntregaResultDto(Guid pedidoId, string enderecoEntrega, string statusPedido)
{
    public Guid PedidoId { get; } = pedidoId;
    public string EnderecoEntrega { get; } = enderecoEntrega;
    public string StatusPedido { get; } = statusPedido;
}
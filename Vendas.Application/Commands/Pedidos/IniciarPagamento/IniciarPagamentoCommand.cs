using Vendas.Domain.Pedidos.Enums;

namespace Vendas.Application.Commands.Pedidos.IniciarPagamento;
public sealed class IniciarPagamentoCommand(Guid pedidoId, MetodoPagamento metodoPagamento)
{
    public Guid PedidoId { get; } = pedidoId;
    public MetodoPagamento MetodoPagamento { get; } = metodoPagamento;
}

using Vendas.Domain.Pedidos.Enums;

namespace Vendas.Application.Queries.Pedidos.ListarPedidosPagamentosPorStatus;
public sealed class ListarPedidosPagamentosPorStatusQuery(StatusPagamento status)
{
    public StatusPagamento Status { get; } = status;
}

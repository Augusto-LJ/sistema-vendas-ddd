using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Queries.Pedidos.DTOs;

namespace Vendas.Application.Queries.Pedidos.ListarPedidosPagamentosPorStatus;
public sealed class ListarPedidosPagamentosPorStatusQueryHandler(IPedidoQueryRepository queryRepository)
{
    private readonly IPedidoQueryRepository _queryRepository = queryRepository;
    public async Task<IReadOnlyList<PagamentoPorStatusDto>> HandleAsync(ListarPedidosPagamentosPorStatusQuery query, CancellationToken cancellationToken)
    {
        return await _queryRepository.ListarPagamentosPorStatusAsync(query.Status, cancellationToken);
    }
}

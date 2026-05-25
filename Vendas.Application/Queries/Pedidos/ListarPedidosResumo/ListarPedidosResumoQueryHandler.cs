using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Queries.Pedidos.DTOs;

namespace Vendas.Application.Queries.Pedidos.ListarPedidosResumo;
public sealed class ListarPedidosResumoQueryHandler(IPedidoQueryRepository queryRepository)
{
    private readonly IPedidoQueryRepository _queryRepository = queryRepository;

    public async Task<IReadOnlyList<PedidoResumoDto>> HandleAsync(ListarPedidosResumoQuery query, CancellationToken cancellationToken)
    {
        return await _queryRepository.ListarResumoAsync(cancellationToken);
    }
}

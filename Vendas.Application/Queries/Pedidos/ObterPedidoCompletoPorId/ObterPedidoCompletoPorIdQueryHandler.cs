using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Queries.Pedidos.DTOs;

namespace Vendas.Application.Queries.Pedidos.ObterPedidoCompletoPorId;
public sealed class ObterPedidoCompletoPorIdQueryHandler(IPedidoQueryRepository queryRepository)
{
    private readonly IPedidoQueryRepository _queryRepository = queryRepository;
    public async Task<PedidoCompletoDto?> HandleAsync(ObterPedidoCompletoPorIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryRepository.ObterPedidoCompletoPorIdAsync(query.PedidoId, cancellationToken);
    }
}

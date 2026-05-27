using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Queries.Pedidos.DTOs;

namespace Vendas.Application.Queries.Pedidos.ListarPedidosResumoPorCliente;
public sealed class ListarPedidosResumoPorClienteQueryHandler(IPedidoQueryRepository queryRepository)
{
    private readonly IPedidoQueryRepository _queryRepository = queryRepository;
    public async Task<IReadOnlyList<PedidoResumoDto>> HandleAsync(ListarPedidosResumoPorClienteQuery query, CancellationToken cancellationToken)
    {
        return await _queryRepository.ListarResumoPorClienteAsync(query.ClienteId, cancellationToken);
    }
}

using Vendas.Application.Queries.Pedidos.DTOs;
using Vendas.Domain.Pedidos.Enums;

namespace Vendas.Application.Abstractions.Persistence;
public interface IPedidoQueryRepository
{
    Task<IReadOnlyList<PedidoResumoDto>> ListarResumoAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PedidoResumoDto>> ListarResumoPorClienteAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PagamentoPorStatusDto>> ListarPagamentosPorStatusAsync(StatusPagamento status, CancellationToken cancellationToken = default);
    Task<PedidoCompletoDto?> ObterPedidoCompletoPorIdAsync(Guid pedidoId, CancellationToken cancellationToken = default);
}

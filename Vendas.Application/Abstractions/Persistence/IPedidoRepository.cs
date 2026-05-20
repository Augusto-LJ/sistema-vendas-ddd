using Vendas.Domain.Pedidos;

namespace Vendas.Application.Abstractions.Persistence;
public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Pedido>> ListarTodosAsync(CancellationToken cancellationToken = default);
}

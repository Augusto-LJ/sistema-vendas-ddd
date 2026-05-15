using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Pedidos;

namespace Vendas.Infrastructure.Fakes;
public class FakePedidoRepository : IPedidoRepository
{
    private readonly Dictionary<Guid, Pedido> _pedidos = [];
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            _pedidos[pedido.Id] = pedido;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            if (!_pedidos.ContainsKey(pedido.Id))
                throw new InvalidOperationException($"Pedido com ID {pedido.Id} não encontrado.");

            _pedidos[pedido.Id] = pedido;
        }
        finally
        {
            _lock.Release();
        }
    }
    public async Task<Pedido?> ObterPorIdAsync(Guid pedidoId, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            _pedidos.TryGetValue(pedidoId, out var pedido);
            return pedido;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<Pedido>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            return _pedidos.Values.ToList().AsReadOnly();
        }
        finally
        {
            _lock.Release();
        }
    }
}

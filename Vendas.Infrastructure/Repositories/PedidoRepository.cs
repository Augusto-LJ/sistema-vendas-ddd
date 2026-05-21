using Microsoft.EntityFrameworkCore;
using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Pedidos;
using Vendas.Infrastructure.Persistence.Context;

namespace Vendas.Infrastructure.Repositories;
public sealed class PedidoRepository(VendasDbContext context) : IPedidoRepository
{
    private readonly VendasDbContext _context = context;

    public async Task<Pedido?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .Include(p => p.Pagamentos)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Pedido>> ListarTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .Include(p => p.Pagamentos)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AdicionarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _context.Pedidos.AddAsync(pedido, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Pedido pedido, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

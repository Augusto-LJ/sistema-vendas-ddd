using Vendas.Domain.Clientes;

namespace Vendas.Application.Abstractions.Persistence;
public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Cliente cliente, CancellationToken cancellationToken = default);
}
using Vendas.Domain.Catalogo;

namespace Vendas.Application.Abstractions.Persistence;
public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(Guid produtoId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Produto produto, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Produto produto, CancellationToken cancellationToken = default);
}

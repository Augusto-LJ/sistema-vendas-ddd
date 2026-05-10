using Vendas.Domain.Catalogo;

namespace Vendas.Application.Abstractions.Persistence;
public interface ICategoriaRepository
{
    Task<Categoria?> ObterPorIdAsync(Guid categoriaId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Categoria categoria, CancellationToken cancellationToken = default);
    Task AtualizarAsync(Categoria categoria, CancellationToken cancellationToken = default);
}

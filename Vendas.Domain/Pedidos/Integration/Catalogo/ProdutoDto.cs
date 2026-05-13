namespace Vendas.Domain.Pedidos.Integration.Catalogo;
public sealed class ProdutoDto(Guid id, string nome, decimal preco)
{
        public Guid Id { get; } = id;
        public string Nome { get; } = nome;
        public decimal Preco { get; } = preco;
}

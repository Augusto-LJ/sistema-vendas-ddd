namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.CriarProduto;
public sealed class CriarProdutoCommand(string nome, string codigo, decimal preco, Guid categoriaId, int estoqueInicial, string? descricao)
{
    public string Nome { get; } = nome;
    public string Codigo { get; } = codigo;
    public decimal Preco { get; } = preco;
    public Guid CategoriaId { get; } = categoriaId;
    public int EstoqueInicial { get; } = estoqueInicial;
    public string? Descricao { get; } = descricao;
}
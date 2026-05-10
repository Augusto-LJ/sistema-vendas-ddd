namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AjustarEstoque;
public sealed class AjustarEstoqueCommand(Guid produtoId, int novoEstoque, string motivo)
{
    public Guid ProdutoId { get; } = produtoId;
    public int NovoEstoque { get; } = novoEstoque;
    public string Motivo { get; } = motivo;
}

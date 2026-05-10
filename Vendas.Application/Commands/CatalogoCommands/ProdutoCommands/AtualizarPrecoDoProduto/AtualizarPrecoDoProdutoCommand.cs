namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizarPrecoDoProduto;
public class AtualizarPrecoDoProdutoCommand(Guid produtoId, decimal novoPreco)
{
    public Guid ProdutoId { get; } = produtoId;
    public decimal NovoPreco { get; } = novoPreco;
}

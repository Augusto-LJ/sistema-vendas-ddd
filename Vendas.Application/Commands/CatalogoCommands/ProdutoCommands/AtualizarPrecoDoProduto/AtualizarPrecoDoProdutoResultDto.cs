namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AtualizarPrecoDoProduto;
public sealed class AtualizarPrecoDoProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public decimal NovoPreco { get; init; }
}

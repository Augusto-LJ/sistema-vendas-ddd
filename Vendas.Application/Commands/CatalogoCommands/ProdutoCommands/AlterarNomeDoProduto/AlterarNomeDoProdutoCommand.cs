namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeDoProduto;
public sealed class AlterarNomeDoProdutoCommand(Guid produtoId, string novoNome)
{
    public Guid ProdutoId { get; } = produtoId;
    public string NovoNome { get; } = novoNome;
}

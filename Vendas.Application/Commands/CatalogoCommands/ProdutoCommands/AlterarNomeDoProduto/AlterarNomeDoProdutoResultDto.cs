namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AlterarNomeDoProduto;
public sealed class AlterarNomeDoProdutoResultDto
{
    public Guid ProdutoId { get; init; }
    public string NovoNome { get; init; }
}

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.AjustarEstoque;
public class AjustarEstoqueResultDto
{
    public Guid ProdutoId { get; init; }
    public int NovoEstoque { get; init; }
    public string Motivo { get; init; }
}

namespace Vendas.Application.Commands.CatalogoCommands.CategoriaCommands.CriarCategoria;
public sealed class CriarCategoriaCommand(string nome, string descricao)
{
    public string Nome { get; } = nome;
    public string Descricao { get; } = descricao;
}

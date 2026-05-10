using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Catalogo;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Application.Commands.CatalogoCommands.ProdutoCommands.CriarProduto;
public sealed class CriarProdutoCommandHandler(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
{
    private readonly IProdutoRepository _produtoRepository = produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository = categoriaRepository;

    public async Task<CriarProdutoResultDto> HandleAsync(CriarProdutoCommand command, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.ObterPorIdAsync(command.CategoriaId, cancellationToken);

        if (categoria is null)
            throw new DomainException("Categoria não encontrada.");

        Guard.Against<DomainException>(!categoria.Ativa, "Não é possível criar um produto em uma categoria inativa");

        var nome = new NomeProduto(command.Nome);
        var codigo = new CodigoProduto(command.Codigo);
        var preco = new PrecoProduto(command.Preco);

        var produto = new Produto(nome, codigo, preco, command.CategoriaId, command.EstoqueInicial, command.Descricao);

        await _produtoRepository.AdicionarAsync(produto, cancellationToken);

        return new CriarProdutoResultDto
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome.Valor,
            Preco = produto.Preco.Valor,
            Status = produto.Status.ToString()
        };
    }
}

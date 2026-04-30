using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Entities;

namespace Vendas.Domain.Tests.Pedidos.Entities;
public class ItemPedidoTests
{
    private static ItemPedido CriarItemValido(decimal preco = 100m, int quantidade = 2)
    {
        return new ItemPedido(Guid.NewGuid(), "Produto teste", preco, quantidade);
    }

    #region Criação
    [Fact]
    public void CriarItemPedido_DeveRetornarItemPedido_QuandoDadosForemValidos()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var nomeProduto = "Produto teste";
        var precoUnitario = 100m;
        var quantidade = 2;

        // Act
        var sut = new ItemPedido(produtoId, nomeProduto, precoUnitario, quantidade);

        // Assert
        sut.Should().NotBeNull();
        sut.ProdutoId.Should().Be(produtoId);
        sut.NomeProduto.Should().Be(nomeProduto);
        sut.PrecoUnitario.Should().Be(precoUnitario);
        sut.Quantidade.Should().Be(quantidade);
        sut.DescontoAplicado.Should().Be(0);
        sut.ValorTotal.Should().Be(precoUnitario * quantidade);
    }

    [Theory]
    [InlineData("", "Produto A", 10, 1, "ProdutoId inválido")]
    [InlineData("guid", "", 10, 1, "O nome do produto é obrigatório")]
    [InlineData("guid", "Produto A", -10, 1, "O preço unitário deve ser maior que zero")]
    [InlineData("guid", "Produto A", 0, 1, "O preço unitário deve ser maior que zero")]
    [InlineData("guid", "Produto A", 10, 0, "A quantidade deve ser maior que zero")]
    public void CriarItemPedido_DeveLancarDomainException_ParametrosForemInvalidos(string tipo, string nomeProduto, decimal preco, int quantidade, string mensagem)
    {
        // Arrange
        var produtoId = tipo == "guid" ? Guid.NewGuid() : Guid.Empty;

        // Act
        Action act = () => new ItemPedido(produtoId, nomeProduto, preco, quantidade);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagem);
    }
    #endregion

    #region Desconto
    [Fact]
    public void AplicarDesconto_DeveAplicarDescontoComSucesso_QuandoValorForValido()
    {
        // Arrange
        var item = CriarItemValido(preco: 200m, quantidade: 2);
        var desconto = 20m;

        // Act
        item.AplicarDesconto(desconto);

        // Assert
        item.DescontoAplicado.Should().Be(desconto);
        item.ValorTotal.Should().Be(item.PrecoUnitario * item.Quantidade - desconto);
        item.DataAtualizacao.Should().NotBeNull();
    }

    [Theory]
    [InlineData(-10, "O desconto não pode ser negativo")]
    [InlineData(500, "O desconto não pode ser maior que o valor total do item")]
    public void AplicarDesconto_DeveLancarDomainException_QuandoValorForInvalido(decimal desconto, string mensagem)
    {
        // Arrange
        var item = CriarItemValido(preco: 100m, quantidade: 2);

        // Act
        Action act = () => item.AplicarDesconto(desconto);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagem);
    }
    #endregion

    #region Adição de unidades
    [Fact]
    public void AdicionarUnidades_DeveAdicionarUnidadesComSucesso_QuandoQuantidadeForValida()
    {
        // Arrange
        var item = CriarItemValido(preco: 50m, quantidade: 2);
        var quantidadeAdicionar = 3;

        // Act
        item.AdicionarUnidades(quantidadeAdicionar);

        // Assert
        item.Quantidade.Should().Be(5);
        item.ValorTotal.Should().Be(item.PrecoUnitario * item.Quantidade - item.DescontoAplicado);
        item.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void AdicionarUnidades_DeveLancarDomainException_QuandoQuantidadeForInvalida()
    {
        // Arrange
        var item = CriarItemValido();

        // Act
        Action act = () => item.AdicionarUnidades(0);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("A quantidade a ser adicionada deve ser maior que zero");
    }
    #endregion

    #region Remoção de unidades
    [Fact]
    public void RemoverUnidades_DeveRemoverUnidadesComSucesso_QuandoQuantidadeForValida()
    {
        // Arrange
        var item = CriarItemValido(preco: 50m, quantidade: 5);
        var quantidadeRemover = 2;

        // Act
        item.RemoverUnidades(quantidadeRemover);

        // Assert
        item.Quantidade.Should().Be(3);
        item.ValorTotal.Should().Be(item.PrecoUnitario * item.Quantidade - item.DescontoAplicado);
        item.DataAtualizacao.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0, "A quantidade a ser removida deve ser maior que zero")]
    [InlineData(5, "A quantidade a ser removida não pode ser maior que a quantidade atual")]
    public void RemoverUnidades_DeveLancarDomainException_QuandoQuantidadeForInvalida(int quantidadeRemover, string mensagem)
    {
        // Arrange
        var item = CriarItemValido(preco: 50m, quantidade: 3);

        // Act
        Action act = () => item.RemoverUnidades(quantidadeRemover);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(mensagem);
    }

    [Fact]
    public void RemoverUnidades_DeveLancarDomainException_QuandoQuantidadeRemoverForIgualAQuantidadeAtual()
    {
        // Arrange
        var item = CriarItemValido(preco: 50m, quantidade: 3);
        var quantidadeRemover = 3;

        // Act
        Action act = () => item.RemoverUnidades(quantidadeRemover);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("A quantidade do item não pode ser zero. Use o método da classe Pedido para removê-lo");
    }
    #endregion

    #region Atualização de preço
    [Fact]
    public void AtualizarPrecoUnitario_DeveAtualizarPrecoComSucesso_QuandoValorForValido()
    {
        // Arrange
        var item = CriarItemValido(preco: 50m, quantidade: 2);
        var novoPreco = 80m;

        // Act
        item.AtualizarPrecoUnitario(novoPreco);

        // Assert
        item.PrecoUnitario.Should().Be(novoPreco);
        item.ValorTotal.Should().Be(item.PrecoUnitario * item.Quantidade);
        item.DataAtualizacao.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AtualizarPrecoUnitario_DeveLancarDomainException_QuandoValorForInvalido(decimal novoPreco)
    {
        // Arrange
        var item = CriarItemValido();

        // Act
        Action act = () => item.AtualizarPrecoUnitario(novoPreco);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O preço unitário deve ser maior que zero");
    }
    #endregion

    #region Igualdade entre entidades
    [Fact]
    public void Equals_DeveRetornarTrue_QuandoItensForemIguais()
    {
        // Arrange
        var item1 = CriarItemValido();
        var item2 = CriarItemValido();
        typeof(ItemPedido).GetProperty("Id")!.SetValue(item2, item1.Id); // Força os dois itens a terem o mesmo Id

        // Act
        var resultado = item1 == item2;

        // Assert
        resultado.Should().BeTrue();
        item1.Equals(item2).Should().BeTrue();
    }
    #endregion
}

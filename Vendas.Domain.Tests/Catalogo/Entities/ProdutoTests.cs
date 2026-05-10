using FluentAssertions;
using Vendas.Domain.Catalogo;
using Vendas.Domain.Catalogo.Enums;
using Vendas.Domain.Catalogo.Events;
using Vendas.Domain.Catalogo.ValueObjects;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Catalogo;
public class ProdutoTests
{
    private Produto CriarProdutoValido(string nome = "Produto teste", string codigo = "Cod-001", decimal preco = 200m, int estoque = 10, string? descricao = null)
    {
        return new Produto(
            new NomeProduto(nome),
            new CodigoProduto(codigo),
            new PrecoProduto(preco),
            Guid.NewGuid(),
            estoque,
            descricao
        );
    }

    #region CriarProduto
    [Fact]
    public void CriarProduto_DeveCriarProduto_QuandoDadosForemValidos()
    {
        // Arrange
        var nome = "Produto teste";
        var codigo = "Cod-001";
        var preco = 200m;
        var estoque = 10;
        var descricao = "Descrição do produto teste";

        // Act
        var produto = CriarProdutoValido(nome, codigo, preco, estoque, descricao);

        // Assert
        produto.Nome.Valor.Should().Be(nome);
        produto.Codigo.Valor.Should().Be(codigo);
        produto.Preco.Valor.Should().Be(preco);
        produto.Estoque.Should().Be(estoque);
        produto.Descricao.Should().Be(descricao);
    }

    [Fact]
    public void CriarProduto_DeveCriarProdutoAtivo_QuandoDadosForemValidos()
    {
        // Arrange & Act
        var produto = CriarProdutoValido();

        // Assert
        produto.Status.Should().Be(StatusProduto.Ativo);
    }

    [Fact]
    public void CriarProduto_DeveLancarDomainException_QuandoEstoqueForNegativo()
    {
        // Arrange & Act
        Action act = () => CriarProdutoValido(estoque: -1);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O estoque inicial não pode ser negativo");
    }
    #endregion

    #region AlterarNome
    [Fact]
    public void AlterarNome_DeveAlterarONome_QuandoNovoNomeForValido()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var novoNome = new NomeProduto("Novo nome do produto");

        // Act
        produto.AlterarNome(novoNome);

        // Assert
        produto.Nome.Valor.Should().Be(novoNome.Valor);
    }

    [Fact]
    public void AlterarNome_DeveLancarDomainException_QuandoNovoNomeForNulo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        Action act = () => produto.AlterarNome(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }
    #endregion

    #region AlterarPreco
    [Fact]
    public void AlterarPreco_DeveAlterarPreco_QuandoNovoPrecoForValido()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var novoPreco = new PrecoProduto(300m);

        // Act
        produto.AlterarPreco(novoPreco);

        // Assert
        produto.Preco.Valor.Should().Be(novoPreco.Valor);
    }

    [Fact]
    public void AlterarPreco_DeveLancarDomainException_QuandoNovoPrecoForNulo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        Action act = () => produto.AlterarPreco(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AlterarPreco_DeveGerarPrecoProdutoAlteradoEvent_QuandoNovoPrecoForValido()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.ClearDomainEvents();
        var novoPreco = new PrecoProduto(300m);

        // Act
        produto.AlterarPreco(novoPreco);

        // Assert
        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<PrecoProdutoAlteradoEvent>();
    }

    [Fact]
    public void AlterarPreco_DeveAtualizarDataAtualizacao_QuandoNovoPrecoForValido()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var novoPreco = new PrecoProduto(300m);

        // Act
        produto.AlterarPreco(novoPreco);

        // Assert
        produto.DataAtualizacao.Should().BeAfter(produto.DataCriacao);
    }
    #endregion

    #region AjustarEstoque
    [Fact]
    public void AjustarEstoque_DeveAjustarOEstoque_QuandoQuantidadeForValida()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var quantidade = 5;
        var motivo = "Reabastecimento de estoque";

        // Act
        produto.AjustarEstoque(quantidade, motivo);

        // Assert
        produto.Estoque.Should().Be(15);
    }

    [Fact]
    public void AjustarEstoque_DeveLancarDomainException_QuandoMotivoForNuloOuVazio()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var quantidade = 5;

        // Act
        Action act = () => produto.AjustarEstoque(quantidade, null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AjustarEstoque_DeveLancarDomainException_QuandoAjusteResultarEmEstoqueNegativo()
    {
        // Arrange
        var produto = CriarProdutoValido(estoque: 5);
        var quantidade = -10;
        var motivo = "Correção de estoque";

        // Act
        Action act = () => produto.AjustarEstoque(quantidade, motivo);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O ajuste de estoque resultaria em um valor negativo");
    }

    [Fact]
    public void AjustarEstoque_DeveAtualizarDataAtualizacao_QuandoQuantidadeForValida()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var quantidade = 5;
        var motivo = "Reabastecimento de estoque";

        // Act
        produto.AjustarEstoque(quantidade, motivo);

        // Assert
        produto.DataAtualizacao.Should().BeAfter(produto.DataCriacao);
    }

    [Fact]
    public void AjustarEstoque_DeveGerarAjusteEstoqueEvent_QuandoQuantidadeForValida()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.ClearDomainEvents();
        var quantidade = 5;
        var motivo = "Reabastecimento de estoque";

        // Act
        produto.AjustarEstoque(quantidade, motivo);

        // Assert
        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<EstoqueAjustadoEvent>();
    }
    #endregion

    #region AlterarDescricao
    [Fact]
    public void AlterarDescricao_DeveAlterarDescricao_QuandoNovaDescricaoForValida()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var novaDescricao = "Nova descrição do produto";

        // Act
        produto.AlterarDescricao(novaDescricao);

        // Assert
        produto.Descricao.Should().Be(novaDescricao);
    }

    [Fact]
    public void AlterarDescricao_DeveAtualizarDataAtualizacao_QuandoNovaDescricaoForValida()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var novaDescricao = "Nova descrição do produto";

        // Act
        produto.AlterarDescricao(novaDescricao);

        // Assert
        produto.DataAtualizacao.Should().BeAfter(produto.DataCriacao);
    }
    #endregion

    #region Ativar
    [Fact]
    public void Ativar_DeveAtivarProduto_QuandoProdutoEstiverInativo()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Inativar();

        // Act
        produto.Ativar();

        // Assert
        produto.Status.Should().Be(StatusProduto.Ativo);
    }

    [Fact]
    public void Ativar_DeveLancarDomainException_QuandoProdutoEstiverAtivo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        Action act = () => produto.Ativar();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O produto já está ativo");
    }

    [Fact]
    public void Ativar_DeveAtualizarDataAtualizacao_QuandoProdutoEstiverInativo()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Inativar();
        var dataAtualizacaoAntes = produto.DataAtualizacao;

        // Act
        produto.Ativar();

        // Assert
        produto.DataAtualizacao.Should().BeAfter((DateTime)dataAtualizacaoAntes!);
    }

    [Fact]
    public void Ativar_DeveGerarProdutoAtivadoEvent_QuandoProdutoEstiverInativo()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Inativar();
        produto.ClearDomainEvents();

        // Act
        produto.Ativar();

        // Assert
        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ProdutoAtivadoEvent>();
    }
    #endregion

    #region Inativar
    [Fact]
    public void Inativar_DeveInativarProduto_QuandoProdutoEstiverAtivo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        produto.Inativar();

        // Assert
        produto.Status.Should().Be(StatusProduto.Inativo);
    }

    [Fact]
    public void Inativar_DeveLancarDomainException_QuandoProdutoEstiverInativo()
    {
        // Arrange
        var produto = CriarProdutoValido();
        produto.Inativar();

        // Act
        Action act = () => produto.Inativar();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O produto já está inativo");
    }

    [Fact]
    public void Inativar_DeveAtualizarDataAtualizacao_QuandoProdutoEstiverAtivo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        produto.Inativar();

        // Assert
        produto.DataAtualizacao.Should().BeAfter(produto.DataCriacao);
    }

    [Fact]
    public void Inativar_DeveGerarProdutoInativadoEvent_QuandoProdutoEstiverAtivo()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        produto.Inativar();

        // Assert
        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ProdutoInativadoEvent>();
    }
    #endregion

    #region AdicionarImagem
    [Fact]
    public void AdicionarImagem_DeveAdicionarImagem_QuandoDadosForemValidos()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var imagem = new ImagemProduto("http://example.com/imagem.jpg", 1);

        // Act
        produto.AdicionarImagem(imagem);

        // Assert
        produto.Imagens.Should().HaveCount(1);
    }

    [Fact]
    public void AdicionarImagem_DeveLancarDomainException_QuandoImagemForNula()
    {
        // Arrange
        var produto = CriarProdutoValido();

        // Act
        Action act = () => produto.AdicionarImagem(null!);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AdicionarImagem_DeveGerarImagemAdicionadaEvent_QuandoDadosForemValidos()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var imagem = new ImagemProduto("http://example.com/imagem.jpg", 1);

        // Act
        produto.AdicionarImagem(imagem);

        // Assert
        produto.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ImagemAdicionadaEvent>();
    }

    [Fact]
    public void AdicionarImagem_DeveLancarDomainException_QuandoJaHouverImagemComMesmaOrdem()
    {
        // Arrange
        var produto = CriarProdutoValido();
        var imagem1 = new ImagemProduto("http://example.com/imagem1.jpg", 1);
        var imagem2 = new ImagemProduto("http://example.com/imagem2.jpg", 1);
        produto.AdicionarImagem(imagem1);
        
        // Act
        Action act = () => produto.AdicionarImagem(imagem2);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Já existe uma imagem nesta ordem");
    }
    #endregion
}

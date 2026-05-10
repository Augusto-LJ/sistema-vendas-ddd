using FluentAssertions;
using Vendas.Domain.Catalogo;
using Vendas.Domain.Catalogo.Events;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Catalogo;
public class CategoriaTests
{
    #region CriarCategoria
    [Fact]
    public void CriarCategoria_DeveCriarCategoria_QuandoDadosForemValidos_()
    {
        // Arrange
        var nome = "Eletrônicos";
        var descricao = "Categoria de produtos eletrônicos";

        // Act
        var categoria = new Categoria(nome, descricao);

        // Assert
        categoria.Ativa.Should().BeTrue();
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().Be(descricao);
        categoria.DataCriacao.Should().NotBe(default);
        categoria.Id.Should().NotBe(Guid.Empty);
        categoria.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void CriarCategoria_DeveCriarCategoria_QuandoDescricaoForNula()
    {
        // Arrange
        var nome = "Eletrônicos";
        string? descricao = null;

        // Act
        var categoria = new Categoria(nome, descricao);

        // Assert
        categoria.Ativa.Should().BeTrue();
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().BeNull();
        categoria.DataCriacao.Should().NotBe(default);
        categoria.Id.Should().NotBe(Guid.Empty);
        categoria.DomainEvents.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void CriarCategoria_DeveLancarDomainException_QuandoNomeForVazio(string? nome)
    {
        // Arrange
        var descricao = "Categoria de produtos eletrônicos";

        // Act
        Action act = () => new Categoria(nome, descricao);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório");
    }

    [Fact]
    public void CriarCategoria_DeveLancarDomainException_QuandoNomeForNulo()
    {
        // Arrange
        string? nome = null;

        // Act
        Action act = () => new Categoria(nome);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório");
    }

    [Fact]
    public void CriarCategoria_DeveLancarDomainException_QuandoNomeForMenorQue3Caracteres()
    {
        // Arrange
        var nome = "AB";
        var descricao = "Categoria de produtos eletrônicos";
        // Act
        Action act = () => new Categoria(nome, descricao);
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome deve conter no mínimo 3 caracteres");
    }
    #endregion

    #region AlterarNome
    [Fact]
    public void AlterarNome_DeveAlterarNome_QuandoNomeForValido()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        var novoNome = "Eletrodomésticos";

        // Act
        categoria.AlterarNome(novoNome);

        // Assert
        categoria.Nome.Should().Be(novoNome);
    }

    [Fact]
    public void AlterarNome_DeveAlterarDataDeAtualizacao_QuandoNomeForValido()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        var dataCriacao = categoria.DataCriacao;
        var novoNome = "Eletrodomésticos";

        // Act
        categoria.AlterarNome(novoNome);

        // Assert
        categoria.DataAtualizacao.Should().BeAfter(dataCriacao);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void AlterarNome_DeveLancarDomainException_QuandoNomeForVazio(string? nome)
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");

        // Act
        Action act = () => categoria.AlterarNome(nome);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório");
    }

    [Fact]
    public void AlterarNome_DeveLancarDomainException_QuandoNomeForNulo()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        string? nome = null;

        // Act
        Action act = () => categoria.AlterarNome(nome);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome é obrigatório");
    }

    [Fact]
    public void AlterarNome_DeveLancarDomainException_QuandoNomeTiverMenosQueTresCaracteres()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        var nome = "AB";

        // Act
        Action act = () => categoria.AlterarNome(nome);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Nome deve conter no mínimo 3 caracteres");
    }
    #endregion

    #region AlterarDescricao
    [Fact]
    public void AlterarDescricao_DeveAlterarDescricao_QuandoDescricaoForValida()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos", "Categoria de produtos eletrônicos");
        var novaDescricao = "Categoria de eletrodomésticos";

        // Act
        categoria.AlterarDescricao(novaDescricao);

        // Assert
        categoria.Descricao.Should().Be(novaDescricao);
    }

    [Fact]
    public void AlterarDescricao_DeveAlterarDataDeAtualizacao_QuandoDescricaoForValida()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos", "Categoria de produtos eletrônicos");
        var dataCriacao = categoria.DataCriacao;
        var novaDescricao = "Categoria de eletrodomésticos";

        // Act
        categoria.AlterarDescricao(novaDescricao);

        // Assert
        categoria.DataAtualizacao.Should().BeAfter(dataCriacao);
    }
    #endregion

    #region Ativar
    [Fact]
    public void Ativar_DeveAtivarCategoria_QuandoCategoriaEstiverInativa()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();

        // Act
        categoria.Ativar();

        // Assert
        categoria.Ativa.Should().BeTrue();
    }

    [Fact]
    public void Ativar_DeveLancarDomainException_QuandoCategoriaEstiverAtiva()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");

        // Act
        Action act = () => categoria.Ativar();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria já está ativa");
    }

    [Fact]
    public void Ativar_DeveAdicionarEventoDeCategoriaAtivada_QuandoCategoriaEstiverInativa()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();
        categoria.ClearDomainEvents();

        // Act
        categoria.Ativar();

        // Assert
        categoria.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CategoriaAtivadaEvent>();
    }

    [Fact]
    public void Ativar_DeveAtualizarDataDeAtualizacao_QuandoCategoriaEstiverInativa()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();
        var dataCriacao = categoria.DataCriacao;

        // Act
        categoria.Ativar();

        // Assert
        categoria.DataAtualizacao.Should().BeAfter(dataCriacao);
    }
    #endregion

    #region Inativar
    [Fact]
    public void Inativar_DeveInativarCategoria_QuandoCategoriaEstiverAtiva()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");

        // Act
        categoria.Inativar();

        // Assert
        categoria.Ativa.Should().BeFalse();
    }

    [Fact]
    public void Inativar_DeveLancarDomainException_QuandoCategoriaEstiverInativa()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();

        // Act
        Action act = () => categoria.Inativar();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Categoria já está inativa");
    }

    [Fact]
    public void Inativar_DeveAdicionarEventoDeCategoriaInativada_QuandoCategoriaEstiverAtiva()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");

        // Act
        categoria.Inativar();

        // Assert
        categoria.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CategoriaInativadaEvent>();
    }

    [Fact]
    public void Inativar_DeveAtualizarDataDeAtualizacao_QuandoCategoriaEstiverAtiva()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        var dataCriacao = categoria.DataCriacao;

        // Act
        categoria.Inativar();

        // Assert
        categoria.DataAtualizacao.Should().BeAfter(dataCriacao);
    }
    #endregion

    #region ClearDomainEvents
    [Fact]
    public void ClearDomainEvents_DeveLimparEventosDeDominio_QuandoChamado()
    {
        // Arrange
        var categoria = new Categoria("Eletrônicos");
        categoria.Inativar();

        // Act
        categoria.ClearDomainEvents();

        // Assert
        categoria.DomainEvents.Should().BeEmpty();
    }
    #endregion
}

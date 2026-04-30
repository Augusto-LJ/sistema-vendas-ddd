using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Entities;
using Vendas.Domain.Pedidos.Enums;
using Vendas.Domain.Pedidos.Events;

namespace Vendas.Domain.Tests.Pedidos.Entities;
public class PagamentoTests
{
    #region CriarPagamento
    [Fact]
    public void CriarPagamento_DeveCriarComStatusPendente_QuandoDadosForemValidos()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var metodoPagamento = MetodoPagamento.CartaoCredito;
        var valor = 100m;

        // Act
        var pagamento = new Pagamento(pedidoId, metodoPagamento, valor);

        // Assert
        pagamento.PedidoId.Should().Be(pedidoId);
        pagamento.MetodoPagamento.Should().Be(metodoPagamento);
        pagamento.Valor.Should().Be(valor);
        pagamento.StatusPagamento.Should().Be(StatusPagamento.Pendente);
        pagamento.DataPagamento.Should().BeNull();
        pagamento.CodigoTransacao.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void CriarPagamento_NaoDeveCriarPagamento_ComValorInvalido(decimal valor)
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        // Act
        Action act = () => new Pagamento(pedidoId, MetodoPagamento.CartaoCredito, valor);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Valor do pagamento deve ser maior que zero");
    }
    #endregion

    #region DefinirCodigoTransacao
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DefinirCodigoTransacao_NaoDeveDefinirCodigo_ComValorInvalido(string codigo)
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.Pix, 100m);

        // Act
        Action act = () => pagamento.DefinirCodigoTransacao(codigo);

        //Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Código de transação inválido");
    }

    [Fact]
    public void DefinirCodigoTransacao_DeveDefinirCodigo_QuandoValorForValido()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.Pix, 100m);
        var codigo = "TRANSAC-123";

        // Act
        pagamento.DefinirCodigoTransacao(codigo);

        // Assert
        pagamento.CodigoTransacao.Should().Be(codigo);
        pagamento.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void DefinirCodigoTransacao_NaoDeveDefinirCodigo_QuandoCodigoJaForDefinido()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.Pix, 100m);
        var codigo = "TRANSAC-123";
        pagamento.DefinirCodigoTransacao(codigo);

        // Act
        Action act = () => pagamento.DefinirCodigoTransacao("OUTRO-CODIGO");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O código de transação já foi definido");
    }
    #endregion

    #region GerarCodigoTransacaoLocal
    [Fact]
    public void GerarCodigoTransacaoLocal_DeveGerarCodigo_QuandoCodigoNaoForDefinido()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.Pix, 100m);

        // Act
        pagamento.GerarCodigoTransacaoLocal();

        // Assert
        pagamento.CodigoTransacao.Should().StartWith("LOCAL-");
        pagamento.CodigoTransacao.Should().HaveLength(14); // "LOCAL-" + 8 caracteres
        pagamento.DataAtualizacao.Should().NotBeNull();
    }
    #endregion

    #region ConfirmarPagamento
    [Fact]
    public void ConfirmarPagamento_DeveAlterarStatusEDataPagamento_QuandoDadosForemValidos()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);
        pagamento.GerarCodigoTransacaoLocal();

        // Act
        pagamento.ConfirmarPagamento();

        // Assert
        pagamento.StatusPagamento.Should().Be(StatusPagamento.Aprovado);
        pagamento.DataPagamento.Should().NotBeNull();
        pagamento.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void ConfirmarPagamento_NaoDeveConfirmar_QuandoStatusForDiferenteDePendente()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);
        pagamento.GerarCodigoTransacaoLocal();
        pagamento.ConfirmarPagamento(); // Primeiro confirma para alterar o status

        // Act
        Action act = () => pagamento.ConfirmarPagamento(); // Tenta confirmar novamente

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Apenas pagamentos pendentes podem ser confirmados");
    }

    [Fact]
    public void ConfirmarPagamento_NaoDeveConfirmar_QuandoCodigoTransacaoNaoForDefinido()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);

        // Act
        Action act = () => pagamento.ConfirmarPagamento();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pagamento não pode ser confirmado sem o código de transação");
    }

    [Fact]
    public void ConfirmarPagamento_DeveGerarEventoDePagamentoAprovado_QuandoDadosForemValidos()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);
        pagamento.GerarCodigoTransacaoLocal();

        // Act
        pagamento.ConfirmarPagamento();

        // Assert
        pagamento.DomainEvents.Should().ContainSingle(e => e is PagamentoAprovadoEvent);
    }
    #endregion

    #region RecusarPagamento
    [Fact]
    public void RecusarPagamento_DeveAlterarStatusEDataPagamento_QuandoDadosForemValidos()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);

        // Act
        pagamento.RecusarPagamento();

        // Assert
        pagamento.StatusPagamento.Should().Be(StatusPagamento.Recusado);
        pagamento.DataPagamento.Should().NotBeNull();
        pagamento.DataAtualizacao.Should().NotBeNull();
    }

    [Fact]
    public void RecusarPagamento_NaoDeveRecusar_QuandoStatusForDiferenteDePendente()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);
        pagamento.RecusarPagamento(); 

        // Act
        Action act = () => pagamento.RecusarPagamento();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Apenas pagamentos pendentes podem ser recusados");
    }

    [Fact]
    public void RecusarPagamento_DeveGerarEventoDePagamentoRecusado_QuandoDadosForemValidos()
    {
        // Arrange
        var pagamento = new Pagamento(Guid.NewGuid(), MetodoPagamento.CartaoCredito, 100m);

        // Act
        pagamento.RecusarPagamento();

        // Assert
        pagamento.DomainEvents.Should().ContainSingle(e => e is PagamentoRejeitadoEvent);
    }
    #endregion
}

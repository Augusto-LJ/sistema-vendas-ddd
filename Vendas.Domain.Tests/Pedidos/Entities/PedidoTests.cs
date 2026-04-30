using FluentAssertions;
using System.Reflection;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Entities;
using Vendas.Domain.Pedidos.Events;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Tests.Pedidos.Entities;
public class PedidoTests
{
    private static EnderecoEntrega CriarEnderecoValido() => EnderecoEntrega.Criar("12345-789", "Rua Exemplo", "Ap exemplo", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "País Exemplo");

    private static readonly Guid ClienteIdValido = Guid.NewGuid();
    private static readonly Guid ProdutoIdValido = Guid.NewGuid();

    private static void SetStatusPedido(Pedido pedido, StatusPedido status)
    {
        typeof(Pedido).GetProperty(nameof(Pedido.StatusPedido), BindingFlags.Public | BindingFlags.Instance)!
            .SetValue(pedido, status);
    }

    #region Criar
    [Fact]
    public void Criar_DeveCriarPedido_QuandoDadosForemValidos()
    {
        // Arrange
        var endereco = CriarEnderecoValido();

        // Act
        var pedido = Pedido.Criar(ClienteIdValido, endereco);

        // Assert
        pedido.Should().NotBeNull();
        pedido.ClienteId.Should().Be(ClienteIdValido);
        pedido.EnderecoEntrega.Should().BeEquivalentTo(endereco);
        pedido.StatusPedido.Should().Be(StatusPedido.Pendente);
        pedido.ValorTotal.Should().Be(0);
        pedido.Itens.Should().BeEmpty();
        pedido.Pagamentos.Should().BeEmpty();
        pedido.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Criar_NaoDeveCriarPedido_QuandoClienteIdForInvalido()
    {
        // Arrange
        var endereco = CriarEnderecoValido();

        // Act
        Action act = () => Pedido.Criar(Guid.Empty, endereco);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("ClienteId inválido");
    }

    [Fact]
    public void Criar_NaoDeveCriarPedido_QuandoEnderecoEntregaForNulo()
    {
        // Arrange
        EnderecoEntrega endereco = null!;

        // Act
        Action act = () => Pedido.Criar(ClienteIdValido, endereco);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço de entrega é obrigatório");
    }
    #endregion

    #region AdicionarItem
    [Fact]
    public void AdicionarItem_DeveAdicionarItem_QuandoDadosForemValidos()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());

        // Act
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 10.5m, 2);

        // Assert
        pedido.Itens.Should().HaveCount(1);
        pedido.ValorTotal.Should().Be(21m);
        pedido.Itens.First().ValorTotal.Should().Be(21m);
    }

    [Fact]
    public void AdicionarItem_DeveAcumularQuantidade_QuandoProdutoJaExistirNoPedido()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());

        // Act
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 10.5m, 2);
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 10.5m, 3);

        // Assert
        pedido.Itens.Should().HaveCount(1);
        var item = pedido.Itens.First();
        item.Quantidade.Should().Be(5);
        item.ValorTotal.Should().Be(52.5m);
        pedido.ValorTotal.Should().Be(52.5m);
        pedido.Itens.First().Quantidade.Should().Be(5);
    }

    [Theory]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]
    public void AdicionarItem_NaoDeveAdicionarItem_QuandoStatusPedidoNaoForPendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, status);

        // Act
        Action act = () => pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 10.5m, 2);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Itens só podem ser adicionados enquanto o pedido está pendente");
    }
    #endregion

    #region RemoverItem
    [Fact]
    public void RemoverItem_NaoDeveRemoverItem_SeHouverApenasUmItem()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 100m, 2);

        // Act
        Action act = () => pedido.RemoverItem(pedido.Itens.First().Id);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido deve conter pelo menos um item");

    }

    [Fact]
    public void RemoverItem_DeveRecalcularValorTotal_QuandoHouverMaisDeUmItem()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        var produto1 = Guid.NewGuid();
        var produto2 = Guid.NewGuid();

        pedido.AdicionarItem(produto1, "Produto 1", 100m, 1);
        pedido.AdicionarItem(produto2, "Produto 2", 200m, 1);

        var itemId = pedido.Itens.First(i => i.ProdutoId == produto1).Id;

        // Act
        pedido.RemoverItem(itemId);

        // Assert
        pedido.Itens.Should().HaveCount(1);
        pedido.ValorTotal.Should().Be(200m);
    }

    [Fact]
    public void RemoverItem_DeveIgnorarRemocao_QuandoItemForInexistente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 100m, 2);

        // Act
        Action act = () => pedido.RemoverItem(Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Item não encontrado no pedido");
    }

    [Theory]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]
    public void RemoverItem_NaoDeveRemoverItem_QuandoStatusPedidoNaoForPendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 100m, 2);
        SetStatusPedido(pedido, status);

        // Act
        Action act = () => pedido.RemoverItem(ProdutoIdValido);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Itens só podem ser removidos de pedidos pendentes");
    }
    #endregion

    #region AtualizarEnderecoEntrega
    [Fact]
    public void AtualizarEnderecoEntrega_DeveAtualizarEndereco_QuandoStatusForPendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido()); // O status inicial é Pendente
        var novoEndereco = EnderecoEntrega.Criar("00000-000", "Logradouro", "Complemento", "Bairro", "Cidade", "Estado", "País");

        // Act
        pedido.AtualizarEnderecoEntrega(novoEndereco);

        // Assert
        pedido.EnderecoEntrega.Should().BeEquivalentTo(novoEndereco);
    }

    [Theory]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]
    public void AtualizarEnderecoEntrega_NaoDeveAtualizarEndereco_QuandoStatusPedidoNaoForPendente(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, status);
        var novoEndereco = EnderecoEntrega.Criar("00000-000", "Logradouro", "Complemento", "Bairro", "Cidade", "Estado", "País");

        // Act
        Action act = () => pedido.AtualizarEnderecoEntrega(novoEndereco);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Endereço de entrega só pode ser atualizado enquanto o pedido está pendente");
    }
    #endregion

    #region IniciarPagamento
    [Fact]
    public void IniciarPagamento_DeveIniciarPagamento_QuandoDadosForemValidos()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 150m, 1);

        // Act
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);

        // Assert
        pagamento.Valor.Should().Be(150m);
        pagamento.StatusPagamento.Should().Be(StatusPagamento.Pendente);
        pedido.Pagamentos.Should().Contain(p => p.Id == pagamento.Id);
    }

    [Fact]
    public void IniciarPagamento_NaoDeveIniciarPagamento_QuandoNaoHouverItensNoPedido()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());

        // Act
        Action act = () => pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Não é possível iniciar o pagamento de um pedido sem itens");
    }

    [Fact]
    public void IniciarPagamento_NaoDeveIniciarPagamento_SeJaHouverPagamentoPendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 150m, 1);
        pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);

        // Act
        Action act = () => pedido.IniciarPagamento(MetodoPagamento.Pix);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Já existe um pagamento pendente para este pedido");
    }
    #endregion

    #region HandlePagamentoAprovado
    [Fact]
    public void HandlePagamentoAprovado_DeveAlterarStatusParaAprovado_AoSerChamado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 150m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);

        // Act
        pedido.HandlePagamentoAprovado(pagamento.Id);

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.PagamentoConfirmado);
    }

    [Theory]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]
    public void HandlePagamentoAprovado_NaoDeveConfirmarPagamento_SeStatusNaoForPendente(StatusPedido statusPedido)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        SetStatusPedido(pedido, statusPedido);

        // Act
        Action act = () => pedido.HandlePagamentoAprovado(pagamento.Id);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido não está no status esperado para confirmação de pagamento");
    }
    #endregion

    #region HandlePagamentoRejeitado
    [Fact]
    public void HandlePagamentoRejeitado_DeveAlterarStatusParaCancelado_AoSerChamado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 150m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);

        // Act
        pedido.HandlePagamentoRejeitado(pagamento.Id);

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
        pedido.DomainEvents.Should().ContainSingle(e => e is PedidoCanceladoEvent);
    }

    [Theory]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.PagamentoConfirmado)]
    [InlineData(StatusPedido.Entregue)]
    [InlineData(StatusPedido.Cancelado)]
    public void HandlePagamentoRejeitado_NaoDeveCancelarPedido_SeStatusNaoForPendente(StatusPedido statusPedido)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 100m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.Pix);
        SetStatusPedido(pedido, statusPedido);

        // Act
        Action act = () => pedido.HandlePagamentoRejeitado(pagamento.Id);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido não está no status esperado para rejeição de pagamento");
    }
    #endregion

    #region MarcarComoEmSeparacao
    [Fact]
    public void MarcarComoEmSeparacao_DeveAlterarStatusParaEmSeparacao_QuandoStatusForPagamentoConfirmado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        pedido.AdicionarItem(ProdutoIdValido, "Produto Exemplo", 150m, 1);
        var pagamento = pedido.IniciarPagamento(MetodoPagamento.CartaoCredito);
        pedido.HandlePagamentoAprovado(pagamento.Id); // Status é definido como PagamentoConfirmado

        // Act
        pedido.MarcarComoEmSeparacao();

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.EmSeparacao);
    }

    [Fact]
    public void MarcarComoEmSeparacao_NaoDeveAlterarStatus_QuandoStatusNaoForPagamentoConfirmado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido()); // Status inicial é Pendente

        // Act
        Action act = () => pedido.MarcarComoEmSeparacao();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido deve ter o pagamento confirmado para ser marcado como em separação");
    }
    #endregion

    #region MarcarComoEnviado
    [Fact]
    public void MarcarComoEnviado_DeveAlterarStatusParaEnviado_QuandoStatusForEmSeparacao()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.EmSeparacao);

        // Act
        pedido.MarcarComoEnviado();

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Enviado);
    }

    [Fact]
    public void MarcarComoEnviado_NaoDeveAlterarStatus_QuandoStatusNaoForEmSeparacao()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido()); // Status inicial é Pendente

        // Act
        Action act = () => pedido.MarcarComoEnviado();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido deve estar em separação para ser marcado como enviado");
    }
    #endregion

    #region MarcarComoEntregue
    [Fact]
    public void MarcarComoEntregue_DeveAlterarStatusParaEntregue_QuandoStatusForEnviado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.Enviado);

        // Act
        pedido.MarcarComoEntregue();

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Entregue);
    }

    [Fact]
    public void MarcarComoEntregue_NaoDeveAlterarStatus_QuandoStatusNaoForEnviado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido()); // Status inicial é Pendente

        // Act
        Action act = () => pedido.MarcarComoEntregue();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido deve estar enviado para ser marcado como entregue");
    }
    #endregion

    #region CancelarPedido
    [Fact]
    public void CancelarPedido_DeveAlterarStatusParaCancelado_QuandoStatusForPendente()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());

        // Act
        pedido.CancelarPedido();

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
    }

    [Fact]
    public void CancelarPedido_DeveAlterarStatusParaCancelado_QuandoStatusForPagamentoConfirmado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.PagamentoConfirmado);

        // Act
        pedido.CancelarPedido();

        // Assert
        pedido.StatusPedido.Should().Be(StatusPedido.Cancelado);
    }

    [Theory]
    [InlineData(StatusPedido.EmSeparacao)]
    [InlineData(StatusPedido.Enviado)]
    [InlineData(StatusPedido.Entregue)]
    public void CancelarPedido_NaoDeveAlterarStatus_QuandoStatusForEmSeparacaoOuEnviadoOuEntregue(StatusPedido status)
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, status);

        // Act
        Action act = () => pedido.CancelarPedido();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Não é possível cancelar um pedido que já está em separação ou posterior");
    }

    [Fact]
    public void CancelarPedido_NaoDeveAlterarStatus_QuandoStatusForCancelado()
    {
        // Arrange
        var pedido = Pedido.Criar(ClienteIdValido, CriarEnderecoValido());
        SetStatusPedido(pedido, StatusPedido.Cancelado);

        // Act
        Action act = () => pedido.CancelarPedido();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O pedido já está cancelado");
    }
    #endregion
}

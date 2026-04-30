using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;
using Vendas.Domain.Pedidos.Events;

namespace Vendas.Domain.Pedidos.Entities;
public sealed class Pagamento : Entity
{
    public Guid PedidoId { get; private set; }
    public MetodoPagamento MetodoPagamento { get; private set; }
    public StatusPagamento StatusPagamento { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime? DataPagamento { get; private set; }
    public string? CodigoTransacao { get; private set; }

    public Pagamento(Guid pedidoId, MetodoPagamento metodoPagamento, decimal valor)
    {
        Guard.AgainstEmptyGuid(pedidoId, nameof(pedidoId), "Pedido inválido");
        Guard.Against<DomainException>(valor <= 0, "Valor do pagamento deve ser maior que zero");
        Guard.Against<DomainException>(!Enum.IsDefined(typeof(MetodoPagamento), metodoPagamento), "Método de pagamento inválido");

        PedidoId = pedidoId;
        MetodoPagamento = metodoPagamento;
        Valor = valor;
        StatusPagamento = StatusPagamento.Pendente;
        DataPagamento = null;
        CodigoTransacao = null;
    }

    public void GerarCodigoTransacaoLocal() // Gerar um código local para não aumentar complexidade com sistemas terceiros (stripe, paypal, etc)
    {
        if (CodigoTransacao is not null)
            return;

        var codigo = $"LOCAL-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        DefinirCodigoTransacao(codigo);
    }

    public void DefinirCodigoTransacao(string codigoTransacao)
    {
        Guard.AgainstNullOrWhiteSpace(codigoTransacao, nameof(codigoTransacao), "Código de transação inválido");
        Guard.Against<DomainException>(CodigoTransacao is not null, "O código de transação já foi definido");
        Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Não é permitido registrar código após confirmação ou recuso do pagamento");
        CodigoTransacao = codigoTransacao;
        SetDataAtualizacao();
    }

    public void ConfirmarPagamento()
    {
        Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Apenas pagamentos pendentes podem ser confirmados");
        Guard.AgainstNullOrWhiteSpace(CodigoTransacao ?? string.Empty, nameof(CodigoTransacao), "O pagamento não pode ser confirmado sem o código de transação");

        StatusPagamento = StatusPagamento.Aprovado;
        DataPagamento = DateTime.UtcNow;
        SetDataAtualizacao();

        AddDomainEvent(new PagamentoAprovadoEvent(Id, PedidoId, Valor, DataPagamento.Value, CodigoTransacao));
    }

    public void RecusarPagamento()
    {
        Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Apenas pagamentos pendentes podem ser recusados");

        StatusPagamento = StatusPagamento.Recusado;
        DataPagamento = DateTime.UtcNow;
        SetDataAtualizacao();

        AddDomainEvent(new PagamentoRejeitadoEvent(Id, PedidoId, Valor, DataPagamento.Value, CodigoTransacao));
    }
}

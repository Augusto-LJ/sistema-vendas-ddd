using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;
using Vendas.Domain.Pedidos.Enums;
using Vendas.Domain.Pedidos.Events;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Entities;
public sealed class Pedido : AggregateRoot
{
    public Guid ClienteId { get; private set; }
    public EnderecoEntrega EnderecoEntrega { get; private set; }
    public decimal ValorTotal { get; private set; }
    public StatusPedido StatusPedido { get; private set; }
    public string NumeroPedido { get; private set; } = string.Empty;

    private readonly List<ItemPedido> _itens = [];
    public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

    private readonly List<Pagamento> _pagamentos = [];
    public IReadOnlyCollection<Pagamento> Pagamentos => _pagamentos.AsReadOnly();

    private Pedido(Guid clienteId, EnderecoEntrega enderecoEntrega)
    {
        Guard.AgainstEmptyGuid(clienteId, nameof(clienteId), "ClienteId inválido");
        Guard.AgainstNull(enderecoEntrega, nameof(enderecoEntrega), "Endereço de entrega é obrigatório");

        ClienteId = clienteId;
        EnderecoEntrega = enderecoEntrega;
        StatusPedido = StatusPedido.Pendente;
        ValorTotal = 0m;

        GerarNumeroPedido();
    }

    public static Pedido Criar(Guid clienteId, EnderecoEntrega enderecoEntrega)
    {
        return new Pedido(clienteId, enderecoEntrega);
    }

    public void AdicionarItem(Guid produtoId, string nomeProduto, decimal precoUnitario, int quantidade)
    {
        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "Itens só podem ser adicionados enquanto o pedido está pendente");

        var pedidoExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (pedidoExistente is not null)
            pedidoExistente.AdicionarUnidades(quantidade);
        else
            _itens.Add(new ItemPedido(produtoId, nomeProduto, precoUnitario, quantidade));

        RecalcularValorTotal();
        SetDataAtualizacao();
    }

    public void RemoverItem(Guid itemId)
    {
        Guard.AgainstEmptyGuid(itemId, nameof(itemId), "ItemId inválido");
        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "Itens só podem ser removidos de pedidos pendentes");

        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        Guard.AgainstNull(item, nameof(item), "Item não encontrado no pedido");

        _itens.Remove(item!);

        Guard.Against<DomainException>(_itens.Count == 0, "O pedido deve conter pelo menos um item");

        RecalcularValorTotal();
        SetDataAtualizacao();
    }

    public void AtualizarEnderecoEntrega(EnderecoEntrega novoEndereco)
    {
        Guard.AgainstNull(novoEndereco, nameof(novoEndereco), "Endereço de entrega é obrigatório");
        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "Endereço de entrega só pode ser atualizado enquanto o pedido está pendente");

        EnderecoEntrega = novoEndereco;
        SetDataAtualizacao();
    }

    public Pagamento IniciarPagamento(MetodoPagamento metodoPagamento)
    {
        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "Pagamento só pode ser iniciado a partir de status Pendente");
        Guard.Against<DomainException>(_itens.Count == 0, "Não é possível iniciar o pagamento de um pedido sem itens");

        if (_pagamentos.Any(p => p.StatusPagamento == StatusPagamento.Pendente))
            throw new DomainException("Já existe um pagamento pendente para este pedido");  

        var novoPagamento = new Pagamento(Id, metodoPagamento, ValorTotal);

        _pagamentos.Add(novoPagamento);

        SetDataAtualizacao();

        return novoPagamento;
    }

    public void HandlePagamentoAprovado(Guid pagamentoId)
    {
        var pagamento = _pagamentos.FirstOrDefault(p => p.Id == pagamentoId);

        if (pagamento is null)
            return;

        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "O pedido não está no status esperado para confirmação de pagamento");

        StatusPedido = StatusPedido.PagamentoConfirmado;
        SetDataAtualizacao();
    }

    public void HandlePagamentoRejeitado(Guid pagamentoId)
    {
        var pagamento = _pagamentos.FirstOrDefault(p => p.Id == pagamentoId);

        if (pagamento is null)
            return;

        Guard.Against<DomainException>(StatusPedido != StatusPedido.Pendente, "O pedido não está no status esperado para rejeição de pagamento");

        StatusPedido = StatusPedido.Cancelado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoCanceladoEvent(Id, ClienteId, StatusPedido, MotivoCancelamento.ErroProcessamentoPagamento, pagamento.Id));
    }

    public void MarcarComoEmSeparacao()
    {
        Guard.Against<DomainException>(StatusPedido != StatusPedido.PagamentoConfirmado, "O pedido deve ter o pagamento confirmado para ser marcado como em separação");

        StatusPedido = StatusPedido.EmSeparacao;
        SetDataAtualizacao();
    }

    public void MarcarComoEnviado()
    {
        Guard.Against<DomainException>(StatusPedido != StatusPedido.EmSeparacao, "O pedido deve estar em separação para ser marcado como enviado");

        StatusPedido = StatusPedido.Enviado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoEnviadoEvent(Id, ClienteId, EnderecoEntrega));
    }

    public void MarcarComoEntregue()
    {
        Guard.Against<DomainException>(StatusPedido != StatusPedido.Enviado, "O pedido deve estar enviado para ser marcado como entregue");

        StatusPedido = StatusPedido.Entregue;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoEntregueEvent(Id, ClienteId));
    }

    public void CancelarPedido(MotivoCancelamento? motivo = null)
    {
        Guard.Against<DomainException>(StatusPedido == StatusPedido.Cancelado, "O pedido já está cancelado");
        Guard.Against<DomainException>(StatusPedido >= StatusPedido.EmSeparacao, "Não é possível cancelar um pedido que já está em separação ou posterior");

        StatusPedido = StatusPedido.Cancelado;
        SetDataAtualizacao();

        AddDomainEvent(new PedidoCanceladoEvent(Id, ClienteId, StatusPedido, motivo ?? MotivoCancelamento.Outro, _pagamentos.LastOrDefault()?.Id));
    }

    private void RecalcularValorTotal()
    {
        ValorTotal = _itens.Sum(i => i.ValorTotal);
    }

    private void GerarNumeroPedido()
    {
        NumeroPedido = $"PED-{Id.ToString()[..8].ToUpper()}";
    }
}
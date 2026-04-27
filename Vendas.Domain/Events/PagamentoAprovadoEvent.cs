namespace Vendas.Domain.Events;
public record PagamentoAprovadoEvent(Guid PagamentoId, Guid PedidoId, decimal Valor, DateTime DataPagamento, string? CodigoTransacao) : DomainEventBase;
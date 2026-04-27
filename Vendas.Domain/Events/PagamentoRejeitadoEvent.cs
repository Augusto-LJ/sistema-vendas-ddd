namespace Vendas.Domain.Events;
public record PagamentoRejeitadoEvent(Guid PagamentoId, Guid PedidoId, decimal Valor, DateTime DataPagamento, string? CodigoTransacao) : DomainEventBase;
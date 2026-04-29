namespace Vendas.Domain.Events;
public sealed record PedidoEntregueEvent(Guid pedidoId, Guid clienteId) : DomainEventBase;
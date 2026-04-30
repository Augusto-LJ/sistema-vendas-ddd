using Vendas.Domain.Common.Base;

namespace Vendas.Domain.Pedidos.Events;
public sealed record PedidoEntregueEvent(Guid pedidoId, Guid clienteId) : DomainEventBase;
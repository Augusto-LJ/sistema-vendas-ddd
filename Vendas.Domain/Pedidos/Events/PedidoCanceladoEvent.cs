using Vendas.Domain.Common.Enums;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Events;
public sealed record PedidoCanceladoEvent(Guid PedidoId, Guid ClienteId, StatusPedido StatusAnterior, MotivoCancelamento MotivoCancelamento, Guid? PagamentoId) : DomainEventBase;
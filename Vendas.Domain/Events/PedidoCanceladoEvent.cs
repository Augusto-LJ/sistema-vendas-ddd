using Vendas.Domain.Common.Enums;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Events;
public sealed record PedidoCanceladoEvent(Guid PedidoId, Guid ClienteId, StatusPedido StatusAnterior, MotivoCancelamento MotivoCancelamento, Guid? PagamentoId) : DomainEventBase;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Events;
public sealed record PedidoEnviadoEvent(Guid PedidoId, Guid clienteId, EnderecoEntrega EnderecoEntrega) : DomainEventBase;

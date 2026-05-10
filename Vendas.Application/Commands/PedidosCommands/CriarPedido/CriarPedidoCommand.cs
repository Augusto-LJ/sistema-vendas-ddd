using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.PedidosCommands.CriarPedido;
public sealed class CriarPedidoCommand(Guid clienteId, EnderecoEntrega enderecoEntrega)
{
    public Guid ClienteId { get; } = clienteId;
    public EnderecoEntrega EnderecoEntrega { get; } = enderecoEntrega;
}

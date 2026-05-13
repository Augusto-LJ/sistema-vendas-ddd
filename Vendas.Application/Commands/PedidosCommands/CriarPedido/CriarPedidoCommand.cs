using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.PedidosCommands.CriarPedido;
public sealed class CriarPedidoCommand(Guid clienteId, Guid enderecoId)
{
    public Guid ClienteId { get; } = clienteId;
    public Guid EnderecoId { get; } = enderecoId;
}

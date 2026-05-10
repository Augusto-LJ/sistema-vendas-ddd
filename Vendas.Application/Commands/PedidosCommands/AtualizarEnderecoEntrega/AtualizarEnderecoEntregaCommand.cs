using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.PedidosCommands.AtualizarEnderecoEntrega;
public sealed class AtualizarEnderecoEntregaCommand(Guid pedidoId, EnderecoEntrega novoEnderecoEntrega)
{
    public Guid PedidoId { get; } = pedidoId;
    public EnderecoEntrega NovoEnderecoEntrega { get; } = novoEnderecoEntrega;
}

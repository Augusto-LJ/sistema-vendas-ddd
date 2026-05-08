using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;
public sealed class AtualizarEnderecoEntregaCommand(Guid pedidoId, EnderecoEntrega novoEnderecoEntrega)
{
    public Guid PedidoId { get; } = pedidoId;
    public EnderecoEntrega NovoEnderecoEntrega { get; } = novoEnderecoEntrega;
}

using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEntregue;
public sealed class MarcarPedidoComoEntregueCommandHandler(IPedidoRepository pedidoRepository)
{
    public async Task<MarcarPedidoComoEntregueResultDto> HandleAsync(MarcarPedidoComoEntregueCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException($"Pedido com ID {command.PedidoId} não encontrado.");

        pedido.MarcarComoEntregue();

        await pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new MarcarPedidoComoEntregueResultDto
        {
            PedidoId = pedido.Id,
            Status = pedido.StatusPedido.ToString()
        };
    }
}

using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.Pedidos.MarcarPedidoComoEnviado;
public sealed class MarcarPedidoComoEnviadoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<MarcarPedidoComoEnviadoResultDto> HandleAsync(MarcarPedidoComoEnviadoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException($"Pedido não encontrado.");

        pedido.MarcarComoEnviado();

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new MarcarPedidoComoEnviadoResultDto
        {
            PedidoId = pedido.Id,
            Status = pedido.StatusPedido.ToString()
        };
    }
}

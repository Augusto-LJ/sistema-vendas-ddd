using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Application.Commands.Pedidos.CancelarPedido;
public sealed class CancelarPedidoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<CancelarPedidoResultDto> HandleAsync(CancelarPedidoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException($"Pedido não encontrado.");

        var motivo = new MotivoCancelamento(command.CodigoMotivo);

        pedido.CancelarPedido(motivo);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new CancelarPedidoResultDto
        {
            PedidoId = pedido.Id,
            Status = pedido.StatusPedido.ToString()
        };
    }
}

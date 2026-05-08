using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Pedidos.RemoverItemDoPedido;
public sealed class RemoverItemDoPedidoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<RemoverItemDoPedidoResultDto> HandleAsync(RemoverItemDoPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new InvalidOperationException("Pedido não encontrado.");

        pedido.RemoverItem(command.ItemId);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new RemoverItemDoPedidoResultDto(pedido.Id, pedido.ValorTotal, pedido.StatusPedido.ToString());
    }
}

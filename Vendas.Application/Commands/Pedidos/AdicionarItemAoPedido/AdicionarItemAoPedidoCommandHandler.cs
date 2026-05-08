using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Pedidos.AdicionarItemAoPedido;
public sealed class AdicionarItemAoPedidoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<AdicionarItemAoPedidoResultDto> HandleAsync(AdicionarItemAoPedidoCommand command, CancellationToken cancellationToken)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new InvalidOperationException("Pedido não encontrado.");

        pedido.AdicionarItem(command.ProdutoId, command.NomeProduto, command.PrecoUnitario, command.Quantidade);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new AdicionarItemAoPedidoResultDto(pedido.Id, pedido.ValorTotal, pedido.StatusPedido.ToString());
    }
}

using Vendas.Application.Abstractions.Persistence;

namespace Vendas.Application.Commands.Pedidos.AtualizarEnderecoEntrega;
public sealed class AtualizarEnderecoEntregaCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<AtualizarEnderecoEntregaResultDto> HandleAsync(AtualizarEnderecoEntregaCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new Exception("Pedido não encontrado.");

        pedido.AtualizarEnderecoEntrega(command.NovoEnderecoEntrega);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new AtualizarEnderecoEntregaResultDto(pedido.Id, pedido.EnderecoEntrega.ToString(), pedido.StatusPedido.ToString());
    }
}

using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Pedidos;

namespace Vendas.Application.Commands.PedidosCommands.CriarPedido;
public sealed class CriarPedidoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<CriarPedidoResultDto> HandleAsync(CriarPedidoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = Pedido.Criar(command.ClienteId, command.EnderecoEntrega);

        await _pedidoRepository.AdicionarAsync(pedido, cancellationToken);

        return new CriarPedidoResultDto(pedido.Id, pedido.NumeroPedido, pedido.DataCriacao, pedido.ValorTotal, pedido.StatusPedido.ToString());
    }
}

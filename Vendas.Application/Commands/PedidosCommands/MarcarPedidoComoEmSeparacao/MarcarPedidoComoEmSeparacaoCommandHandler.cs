using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.PedidosCommands.MarcarPedidoComoEmSeparacao;
public sealed class MarcarPedidoComoEmSeparacaoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<MarcarPedidoComoEmSeparacaoResultDto> HandleAsync(MarcarPedidoComoEmSeparacaoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException($"Pedido não encontrado");

        pedido.MarcarComoEmSeparacao();

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new MarcarPedidoComoEmSeparacaoResultDto
        {
            PedidoId = pedido.Id,
            StatusPedido = pedido.StatusPedido.ToString()
        };
    }
}

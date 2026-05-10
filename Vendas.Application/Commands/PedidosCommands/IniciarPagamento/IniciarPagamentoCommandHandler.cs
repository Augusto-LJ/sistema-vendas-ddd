using Vendas.Application.Abstractions.Persistence;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Application.Commands.PedidosCommands.IniciarPagamento;
public sealed class IniciarPagamentoCommandHandler(IPedidoRepository pedidoRepository)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<IniciarPagamentoResultDto> HandleAsync(IniciarPagamentoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException("Pedido não encontrado.");

        var pagamento = pedido.IniciarPagamento(command.MetodoPagamento);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new IniciarPagamentoResultDto
        {
            PedidoId = pedido.Id,
            PagamentoId = pagamento.Id,
            StatusPedido = pedido.StatusPedido.ToString(),
            StatusPagamento = pagamento.StatusPagamento.ToString()
        };
    }
}

using Vendas.Application.Abstractions.Persistence;
using Vendas.Application.Commands.PedidosCommands.AdicionarItemAoPedido;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.Integration.Catalogo;

namespace Vendas.Application.Commands.Pedidos.AdicionarItemAoPedido;
public sealed class AdicionarItemAoPedidoCommandHandler(IPedidoRepository pedidoRepository, ICatalogoGateway catalogoGateway, CatalogoAcl catalogoAcl)
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly ICatalogoGateway _catalogoGateway = catalogoGateway;
    private readonly CatalogoAcl _catalogoAcl = catalogoAcl;

    public async Task<AdicionarItemAoPedidoResultDto> HandleAsync(AdicionarItemAoPedidoCommand command, CancellationToken cancellationToken = default)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(command.PedidoId, cancellationToken);

        if (pedido is null)
            throw new DomainException("Pedido não encontrado.");

        var produtoDto = await _catalogoGateway.ObterProdutoPorIdAsync(command.ProdutoId, cancellationToken);

        if (produtoDto is null)
            throw new DomainException("Produto não encontrado");

        var (nomeProduto, precoUnitario) = _catalogoAcl.TraduzirProduto (produtoDto);

        pedido.AdicionarItem(command.ProdutoId, nomeProduto, precoUnitario, command.Quantidade);

        await _pedidoRepository.AtualizarAsync(pedido, cancellationToken);

        return new AdicionarItemAoPedidoResultDto(pedido.Id, pedido.ValorTotal, pedido.StatusPedido.ToString());
    }
}

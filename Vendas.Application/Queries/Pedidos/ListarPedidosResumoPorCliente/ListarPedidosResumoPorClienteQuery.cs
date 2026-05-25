namespace Vendas.Application.Queries.Pedidos.ListarPedidosResumoPorCliente;
public sealed class ListarPedidosResumoPorClienteQuery
{
    public Guid ClienteId { get; }

    public ListarPedidosResumoPorClienteQuery(Guid clienteId)
    {
        if (clienteId == Guid.Empty)
            throw new ArgumentException("O ID do cliente não pode ser vazio.", nameof(clienteId));

        ClienteId = clienteId;
    }
}

namespace Vendas.Domain.Pedidos.Integration.Cliente;
public interface IClientesGateway
{
    Task<EnderecoDto> ObterEnderecoAsync(Guid clienteId, Guid enderecoId, CancellationToken cancellationToken = default);
}

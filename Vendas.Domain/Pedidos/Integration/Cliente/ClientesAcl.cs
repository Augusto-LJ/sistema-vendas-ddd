using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Cliente;
public sealed class ClientesAcl
{
    private readonly IClientesGateway _gateway;

    public ClientesAcl(IClientesGateway gateway)
    {
        Guard.AgainstNull(gateway, nameof(gateway));
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
    }

    public async Task<EnderecoEntregaSnapshot> ObterEnderecoEntregaSnapshotAsync(Guid clienteId, Guid enderecoId, CancellationToken cancellationToken = default)
    {
        var clienteDto = await _gateway.ObterEnderecoAsync(clienteId, enderecoId, cancellationToken);

        if (clienteDto is null)
            throw new DomainException($"Cliente não encontrado");

        return new EnderecoEntregaSnapshot(clienteDto.Cep, clienteDto.Logradouro, clienteDto.Numero, clienteDto.Complemento, clienteDto.Bairro, clienteDto.Cidade, clienteDto.Estado, clienteDto.Pais);
    }
}

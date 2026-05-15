using Vendas.Domain.Pedidos.Integration.Cliente;

namespace Vendas.Infrastructure.Fakes;
public sealed class FakeClientesGateway : IClientesGateway
{
    private static readonly Dictionary<Guid, Dictionary<Guid, EnderecoDto>> _clientes = new()
    {
        [new Guid("11000000-0000-0000-0000-000000000000")] = new()
        {
            [new Guid("21000000-0000-0000-0000-000000000000")] =
                new(
                    id: new Guid("31000000-0000-0000-0000-000000000000"),
                    cep: "00000-000",
                    logradouro: "Rua Exemplo",
                    numero: "123",
                    complemento: "Apto 101",
                    bairro: "Centro",
                    cidade: "Cidade Exemplo",
                    estado: "Estado Exemplo",
                    pais: "Brasil"
                ),
            [new Guid("22000000-0000-0000-0000-000000000000")] =
                new(
                    id: new Guid("32000000-0000-0000-0000-000000000000"),
                    cep: "00001-000",
                    logradouro: "Rua Exemplo Diferente",
                    numero: "12",
                    complemento: "Apto 103",
                    bairro: "Santana",
                    cidade: "Cidade Movimentada",
                    estado: "SP",
                    pais: "Brazil"
                ),
            [new Guid("23000000-0000-0000-0000-000000000000")] =
                new(
                    id: new Guid("33000000-0000-0000-0000-000000000000"),
                    cep: "00021-000",
                    logradouro: "Rua Exemplo Diferente Novamente",
                    numero: "12762",
                    complemento: "Apto 999",
                    bairro: "Leblon",
                    cidade: "Cidade Maravilhosa",
                    estado: "RJ",
                    pais: "Brasil"
                ),
        }
    };

    public Task<EnderecoDto?> ObterEnderecoAsync(Guid clienteId, Guid enderecoId, CancellationToken cancellationToken = default)
    {
        if (_clientes.TryGetValue(clienteId, out var enderecos) && enderecos.TryGetValue(enderecoId, out var endereco))
            return Task.FromResult<EnderecoDto?>(endereco);

        return Task.FromResult<EnderecoDto?>(null);
    }
}

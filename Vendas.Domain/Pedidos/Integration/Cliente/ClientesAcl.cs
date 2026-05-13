using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Integration.Cliente;
public sealed class ClientesAcl
{
    public EnderecoEntrega TraduzirEndereco(EnderecoDto enderecoDto)
    {
        return EnderecoEntrega.Criar(
            enderecoDto.Cep,
            enderecoDto.Logradouro,
            enderecoDto.Numero,
            enderecoDto.Complemento,
            enderecoDto.Bairro,
            enderecoDto.Cidade,
            enderecoDto.Estado,
            enderecoDto.Pais
        );
    }
}

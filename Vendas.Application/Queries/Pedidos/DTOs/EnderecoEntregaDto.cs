namespace Vendas.Application.Queries.Pedidos.DTOs;
public sealed class EnderecoEntregaDto
{
    public string Cep { get; init; } = string.Empty;
    public string Logradouro { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string Complemento { get; init; } = string.Empty;
    public string Bairro { get; init; } = string.Empty;
    public string Cidade { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Pais { get; init; } = string.Empty;
}

namespace Vendas.Domain.Pedidos.Integration.Cliente;
public sealed class EnderecoDto(Guid id, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string cep, string pais)
{
    public Guid Id { get; } = id;
    public string Cep { get; } = cep;
    public string Logradouro { get; } = logradouro;
    public string Numero { get; } = numero;
    public string Complemento { get; } = complemento;
    public string Bairro { get; } = bairro;
    public string Cidade { get; } = cidade;
    public string Estado { get; } = estado;
    public string Pais { get; } = pais;
}

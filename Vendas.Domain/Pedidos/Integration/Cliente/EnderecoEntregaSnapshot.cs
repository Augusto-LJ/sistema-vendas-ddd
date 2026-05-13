using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Integration.Cliente;
public sealed class EnderecoEntregaSnapshot : ValueObject
{
    public string Cep { get; }
    public string Logradouro { get; }
    public string Numero { get; }
    public string Complemento { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }
    public string Pais { get; }

    public EnderecoEntregaSnapshot(string cep, string logradouro, string numero, string complemento, string bairro, string cidade, string estado, string pais)
    {
        Guard.AgainstNullOrWhiteSpace(cep, nameof(cep), "CEP é obrigatório");
        Guard.AgainstNullOrWhiteSpace(logradouro, nameof(logradouro), "Logradouro é obrigatório");
        Guard.AgainstNullOrWhiteSpace(numero, nameof(numero), "Número é obrigatório");
        Guard.AgainstNullOrWhiteSpace(bairro, nameof(bairro), "Bairro é obrigatório");
        Guard.AgainstNullOrWhiteSpace(cidade, nameof(cidade), "Cidade é obrigatória");
        Guard.AgainstNullOrWhiteSpace(estado, nameof(estado), "Estado é obrigatório");
        Guard.AgainstNullOrWhiteSpace(pais, nameof(pais), "País é obrigatório");

        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento ?? string.Empty;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Numero;
        yield return Complemento;
        yield return Bairro;
        yield return Cidade;
        yield return Estado;
        yield return Pais;
    }
}

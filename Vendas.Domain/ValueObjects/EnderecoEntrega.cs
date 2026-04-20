using System.Text.RegularExpressions;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.ValueObjects;
public class EnderecoEntrega : ValueObject
{
    public string Cep { get; private set; }
    public string Logradouro { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Pais { get; private set; }

    private EnderecoEntrega(string cep, string logradouro, string complemento, string bairro, string cidade, string estado, string pais)
    {
        Guard.AgainstNullOrWhiteSpace(cep, nameof(cep));
        Guard.AgainstNullOrWhiteSpace(logradouro, nameof(logradouro));
        Guard.AgainstNullOrWhiteSpace(complemento, nameof(complemento));
        Guard.AgainstNullOrWhiteSpace(bairro, nameof(bairro));
        Guard.AgainstNullOrWhiteSpace(cidade, nameof(cidade));
        Guard.AgainstNullOrWhiteSpace(estado, nameof(estado));
        Guard.AgainstNullOrWhiteSpace(pais, nameof(pais));

        // Validação do CEP
        if (!Regex.IsMatch(cep ?? "", @"^\d{5}-\d{3}$"))
            throw new DomainException("CEP inválido. Deve estar no formato 00000-000");

        Cep = cep!;
        Logradouro = logradouro;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Pais = pais;
    }

    public static EnderecoEntrega Criar(string cep, string logradouro, string complemento, string bairro, string cidade, string estado, string pais)
    {
        return new EnderecoEntrega(cep, logradouro, complemento, bairro, cidade, estado, pais);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Logradouro;
        yield return Complemento ?? string.Empty;
        yield return Bairro;
        yield return Cidade;
        yield return Estado;
        yield return Pais;
    }

    public string FormatarEndereco()
    {
        return $"{Logradouro}, {Complemento} - {Bairro}, {Cidade} - {Estado}, {Pais} - CEP: {Cep}";
    }
}

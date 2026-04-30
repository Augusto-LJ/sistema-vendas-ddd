using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Clientes.ValueObjects;
public sealed class NomeCompleto : ValueObject
{
    public string Nome { get; }
    public string Sobrenome { get; }
    public string NomeCompletoFormatado { get; }

    public NomeCompleto(string nomeCompleto)
    {
        Guard.AgainstNullOrWhiteSpace(nomeCompleto, nameof(nomeCompleto), "O nome completo é obrigatório");

        var partes = nomeCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Guard.Against<DomainException>(partes.Length < 2, "O nome completo deve conter pelo menos nome e sobrenome");

        Nome = string.Join(' ', partes.Take(partes.Length - 1));
        Sobrenome = partes.Last();
        NomeCompletoFormatado = string.Join(" ", partes);
    }

    public string NomeResumido()
    {
        return $"{Nome.Split(' ').First()} {Sobrenome}";
    }

    public override string ToString()
    {
        return NomeCompletoFormatado;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NomeCompletoFormatado.ToLowerInvariant();
    }
}
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Clientes.ValueObjects;
public sealed class Telefone : ValueObject
{
    public string Numero { get; }
    public Telefone(string numero)
    {
        Guard.AgainstNullOrWhiteSpace(numero, nameof(numero), "O número de telefone é obrigatório");

        var digitos = new string(numero.Where(char.IsDigit).ToArray());

        Guard.Against<DomainException>(digitos.Length < 10 || digitos.Length > 11, "Número de telefone deve conter 10 (fixo) ou 11 (celular) dígitos");

        Numero = digitos;
    }

    public override string ToString()
    {
        if (Numero.Length == 11)
            return Convert.ToUInt64(Numero).ToString(@"\(00\) 00000\-0000");

        return Convert.ToUInt64(Numero).ToString(@"\(00\) 0000\-0000");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Numero;
    }
}

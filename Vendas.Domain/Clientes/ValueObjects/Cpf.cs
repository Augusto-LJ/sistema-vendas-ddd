using System.Globalization;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Clientes.ValueObjects;
public sealed class Cpf :ValueObject
{
    public string Numero { get; }
    public Cpf(string numero)
    {
        Guard.AgainstNullOrWhiteSpace(numero, nameof(numero), "O CPF é obrigatório");

        var digitos = new string(numero.Where(char.IsDigit).ToArray());

        Guard.Against<DomainException>(digitos.Length != 11, "CPF deve conter 11 dígitos");
        Guard.Against<DomainException>(!CpfEhValido(digitos), "CPF inválido");

        Numero = numero;
    }

    public override string ToString()
    {
        return Convert.ToUInt64(Numero).ToString(@"000\.000\.000\-00");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Numero;
    }

    private static bool CpfEhValido(string cpf)
    {
        if (new string(cpf[0], cpf.Length) == cpf)
            return false;

        int Soma(int tamanho, int peso)
        {
            int soma = 0;
            for (int i = 0; i < tamanho; i++)
            {
                soma += (cpf[i] - '0') * (peso - i);
            }

            return soma;
        }

        var soma1 = Soma(9, 10) % 11;
        soma1 = soma1 < 2 ? 0 : 11 - soma1;

        var soma2 = Soma(10, 11) % 11;
        soma2 = soma2 < 2 ? 0 : 11 - soma2;

        return cpf[9] - '0' == soma1 && cpf[10] - '0' == soma2;
    }
}

using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.ValueObjects;
public sealed class MotivoCancelamento : ValueObject
{
    public string Codigo { get; }
    public string Descricao { get; }

    private static readonly Dictionary<string, string> _motivosCancelamento = new()
    {
        { "ClienteDesistiu", "Cliente desistiu da compra" },
        { "ErroProcessamentoPagamento", "Erro no processamento do pagamento" },
        { "ItemEsgotadoEstoque", "Item esgotado no estoque" },
        { "EnderecoEntregaInvalido", "Endereço de entrega inválido" },
        { "Outro", "Outro motivo não especificado" }
    };

    public MotivoCancelamento(string codigo)
    {
        Guard.AgainstNullOrWhiteSpace(codigo, nameof(codigo));

        if (!_motivosCancelamento.TryGetValue(codigo, out string? value))
            throw new DomainException($"Motivo de cancelamento inválido: {codigo}");

        Codigo = codigo;
        Descricao = value;
    }

    public static MotivoCancelamento ClienteDesistiu => new("ClienteDesistiu");
    public static MotivoCancelamento ErroProcessamentoPagamento => new("ErroProcessamentoPagamento");
    public static MotivoCancelamento ItemEsgotadoEstoque => new("ItemEsgotadoEstoque");
    public static MotivoCancelamento EnderecoEntregaInvalido => new("EnderecoEntregaInvalido");
    public static MotivoCancelamento Outro => new("Outro");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Codigo;
        yield return Descricao;
    }

    public override string ToString() => $"{Descricao}";
}

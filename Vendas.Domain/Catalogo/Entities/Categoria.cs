using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Catalogo;
public sealed class Categoria : AggregateRoot
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public bool Ativa { get; private set; }

    public Categoria(string nome, string? descricao = null)
    {
        Guard.AgainstNullOrWhiteSpace(nome, nameof(nome), "Nome é obrigatório");
        Guard.Against<DomainException>(nome.Length < 3, "Nome deve conter no mínimo 3 caracteres");

        Nome = nome.Trim();
        Descricao = descricao;
        Ativa = true;
    }

    public void AlterarNome(string nome)
    {
        Guard.AgainstNullOrWhiteSpace(nome, nameof(nome), "Nome é obrigatório");
        Guard.Against<DomainException>(nome.Length < 3, "Nome deve conter no mínimo 3 caracteres");

        Nome = nome.Trim();
        SetDataAtualizacao();
    }

    public void AlterarDescricao(string? descricao)
    {
        Descricao = descricao;
        SetDataAtualizacao();
    }

    public void Ativar()
    {
        Guard.Against<DomainException>(Ativa, "Categoria já está ativa");

        Ativa = true;
        SetDataAtualizacao();
        AddDomainEvent(new Events.CategoriaAtivadaEvent(Id));
    }

    public void Inativar()
    {
        Guard.Against<DomainException>(!Ativa, "Categoria já está inativa");

        Ativa = false;
        SetDataAtualizacao();
        AddDomainEvent(new Events.CategoriaInativadaEvent(Id));
    }
}

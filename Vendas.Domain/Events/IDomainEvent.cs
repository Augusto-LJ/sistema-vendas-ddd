namespace Vendas.Domain.Events;
public interface IDomainEvent
{
    DateTime DateOcurred { get; }
}

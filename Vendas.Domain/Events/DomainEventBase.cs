
namespace Vendas.Domain.Events;
public abstract record class DomainEventBase : IDomainEvent
{ 
    public DateTime DateOcurred { get; protected set; } = DateTime.UtcNow;
}

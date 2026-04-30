using Vendas.Domain.Common.Base;

namespace Vendas.Domain.Clientes.Events;
public sealed record EnderecoPrincipalAlterado(Guid ClienteId, Guid NovoEnderecoId) : DomainEventBase;
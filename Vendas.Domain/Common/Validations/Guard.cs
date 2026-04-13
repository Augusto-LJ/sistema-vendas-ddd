using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Common.Validations;
internal static class Guard
{
    public static void AgainstNull<T>(T value, string paramName)
    {
        if (value is null)
            throw new DomainException($"{paramName} não pode ser nulo");
    }
    public static void AgainstEmptyGuid(Guid id, string paramName)
    {
        if (id == Guid.Empty)
            throw new DomainException($"{paramName} não pode ser Guid.Empty");
    }

    public static void Against<TException>(bool condition, string message) where TException : Exception
    {
        if (condition)
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}

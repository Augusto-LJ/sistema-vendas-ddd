namespace Vendas.Domain.Common.Exceptions;
public class DomainException(string message) : Exception(message)
{
    public static void When(bool hasError, string errorMessage)
    {
        if (hasError)
            throw new DomainException(errorMessage);
    }
}

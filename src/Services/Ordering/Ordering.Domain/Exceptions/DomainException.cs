namespace Ordering.Domain.Exceptions;

public class DomainException : ApplicationException
{
    public DomainException(string message)
        : base($"Domain Exception: \"{message}\" throws from Domain Layer")
    {

    }
}

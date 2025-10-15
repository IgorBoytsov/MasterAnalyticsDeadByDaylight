using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.Exceptions
{
    public sealed class IdentifierOutOfRangeException(Error error) : DomainException(error);
}
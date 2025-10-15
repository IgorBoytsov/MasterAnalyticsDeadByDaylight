using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.Exceptions
{
    public sealed class LengthException(Error error) : DomainException(error);
}
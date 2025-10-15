using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.Exceptions
{
    public sealed class NameException(Error error) : DomainException(error);
}
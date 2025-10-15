using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.Exceptions
{
    public sealed class NotFoundException(Error error) : DomainException(error);
}
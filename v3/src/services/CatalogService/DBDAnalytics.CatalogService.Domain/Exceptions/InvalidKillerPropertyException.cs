using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.Exceptions
{
    public sealed class InvalidKillerPropertyException(string message) : DomainException(new Error(ErrorCode.Validation, message));
}
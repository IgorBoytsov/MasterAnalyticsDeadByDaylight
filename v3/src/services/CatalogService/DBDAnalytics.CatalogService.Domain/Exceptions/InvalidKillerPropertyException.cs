using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;

namespace DBDAnalytics.CatalogService.Domain.Exceptions
{
    public sealed class InvalidKillerPropertyException(string message) : DomainException(new Error(ErrorCode.Validation, message));
}
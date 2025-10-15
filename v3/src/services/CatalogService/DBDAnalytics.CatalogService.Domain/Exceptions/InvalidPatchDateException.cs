using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.Exceptions
{
    public class InvalidPatchDateException(Error error) : DomainException(error);
}
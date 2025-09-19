using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Update
{
    public sealed record UpdateTypeDeathCommand(int TypeDeathId, string Name) : IRequest<Result>,
        IHasName;
}
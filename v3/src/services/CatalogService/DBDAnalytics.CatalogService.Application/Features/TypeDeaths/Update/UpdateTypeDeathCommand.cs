using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Update
{
    public sealed record UpdateTypeDeathCommand(int TypeDeathId, string Name) : IRequest<Result>,
        IHasName;
}
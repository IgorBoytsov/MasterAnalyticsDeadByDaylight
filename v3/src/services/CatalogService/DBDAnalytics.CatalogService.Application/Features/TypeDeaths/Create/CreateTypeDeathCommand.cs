using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Create
{
    public sealed record CreateTypeDeathCommand(int OldId, string Name) : IRequest<Result<TypeDeathResponse>>,
        IHasName;
}
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Create
{
    public sealed record CreateTypeDeathCommand(int OldId, string Name) : IRequest<Result<TypeDeathResponse>>,
        IHasName;
}
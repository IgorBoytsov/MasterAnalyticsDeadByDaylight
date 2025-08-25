using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Create
{
    public sealed record CreateSurvivorCommand(int OldId, string Name, FileInput? Image, string SemanticImageName, List<CreateSurvivorPerkCommandData> Perks) : IRequest<Result<SurvivorResponse>>,
        IHasName,
        IHasSemanticImageName;

    public sealed record CreateSurvivorPerkCommandData(int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
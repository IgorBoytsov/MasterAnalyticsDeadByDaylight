using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Create
{
    public sealed record CreateSurvivorPerkCommand(List<AddSurvivorPerkCommandData> Perks) : IRequest<Result<List<SurvivorPerkResponse>>>,
        IHasPerks<AddSurvivorPerkCommandData>;

    public sealed record AddSurvivorPerkCommandData(Guid SurvivorId, int OldId, string Name, FileInput? Image, string SemanticImageName) : 
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
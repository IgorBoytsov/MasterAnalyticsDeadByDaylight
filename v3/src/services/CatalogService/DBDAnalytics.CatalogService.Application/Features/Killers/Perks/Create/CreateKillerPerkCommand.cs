using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Create
{
    public sealed record CreateKillerPerkCommand(List<AddPerkToKillerCommandData> Perks) : IRequest<Result<List<KillerPerkResponse>>>,
        IHasPerks<AddPerkToKillerCommandData>;

    public sealed record AddPerkToKillerCommandData(Guid KillerId, int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasKillerId,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
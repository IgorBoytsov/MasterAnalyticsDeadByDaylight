using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AddAddon
{
    public sealed record CreateKillerAddonCommand(List<AddAddonToKillerCommandData> Addons) : IRequest<Result<List<KillerAddonResponse>>>,
        IHasAddons<AddAddonToKillerCommandData>;

    public sealed record AddAddonToKillerCommandData(Guid KillerId, int OldId, string Name, FileInput? Image, string SemanticImageName) :
        IHasKillerId,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
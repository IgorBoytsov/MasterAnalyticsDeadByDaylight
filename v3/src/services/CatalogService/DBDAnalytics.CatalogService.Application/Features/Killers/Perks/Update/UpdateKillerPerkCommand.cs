using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Update
{
    public sealed record UpdateKillerPerkCommand(Guid KillerId, Guid PerkId, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result<string>>,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
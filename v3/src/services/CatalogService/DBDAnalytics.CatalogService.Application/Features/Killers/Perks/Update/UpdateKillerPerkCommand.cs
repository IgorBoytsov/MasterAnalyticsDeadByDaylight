using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Update
{
    public sealed record UpdateKillerPerkCommand(Guid KillerId, Guid PerkId, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result>,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
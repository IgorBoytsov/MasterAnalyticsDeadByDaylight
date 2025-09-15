using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Update
{
    public sealed record UpdateKillerAddonCommand(Guid KillerId, Guid AddonId, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result<string>>,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
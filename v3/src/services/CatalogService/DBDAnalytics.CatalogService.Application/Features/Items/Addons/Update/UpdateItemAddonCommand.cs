using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Update
{
    public sealed record UpdateItemAddonCommand(Guid Id, Guid ItemAddonId, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result<string>>,
        IHasGuidId,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
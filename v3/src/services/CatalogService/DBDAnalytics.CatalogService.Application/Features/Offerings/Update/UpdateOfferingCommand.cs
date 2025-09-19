using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Update
{
    public sealed record UpdateOfferingCommand(
        Guid Id, string Name, FileInput? Image, string SemanticImageName,
        int RoleId, int? RarityId, int? CategoryId) : IRequest<Result<string>>,
        IHasGuidId,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
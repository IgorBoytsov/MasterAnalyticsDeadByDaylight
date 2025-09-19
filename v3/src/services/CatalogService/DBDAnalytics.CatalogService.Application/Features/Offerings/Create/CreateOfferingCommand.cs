using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Create
{
    public sealed record CreateOfferingCommand(int OldId, string Name, FileInput? Image, string SemanticImageName, int RoleId, int? RarityId, int? CategoryId) : IRequest<Result<OfferingResponse>>,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName,
        IHasRoleId,
        IMayHasCategoryId;
}
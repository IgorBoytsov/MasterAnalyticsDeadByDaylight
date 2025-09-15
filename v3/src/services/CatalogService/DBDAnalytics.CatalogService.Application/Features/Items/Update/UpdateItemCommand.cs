using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Update
{
    public sealed record UpdateItemCommand(Guid Id, string Name, FileInput? Image, string SemanticImageName) : IRequest<Result<string>>,
        IHasGuidId,
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
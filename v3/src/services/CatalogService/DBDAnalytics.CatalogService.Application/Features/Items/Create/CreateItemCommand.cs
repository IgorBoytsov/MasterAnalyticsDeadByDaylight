using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Create
{
    public sealed record CreateItemCommand(int OldId, string Name, FileInput? Image, string SemanticImageName, List<CreateItemAddonCommandData> Addons) : IRequest<Result<ItemResponse>>,
        IHasName,
        IHasSemanticImageName;

    public sealed record CreateItemAddonCommandData(int OldId, string Name, FileInput? Image, string SemanticImageName, int? RarityId) :
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
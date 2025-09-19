using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Create
{
    public sealed record CreateItemAddonCommand(List<AddItemAddonCommandData> Addons) : IRequest<Result<List<ItemAddonResponse>>>,
        IHasAddons<AddItemAddonCommandData>;

    public sealed record AddItemAddonCommandData(Guid ItemId, int OldId, string Name, FileInput? Image, string SemanticImageName, int? RarityId) :
        IHasName,
        IMayHasFileInput,
        IHasSemanticImageName;
}
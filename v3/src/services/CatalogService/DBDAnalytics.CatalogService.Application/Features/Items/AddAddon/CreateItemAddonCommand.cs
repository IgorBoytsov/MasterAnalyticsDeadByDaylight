using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Items.AddAddon
{
    public sealed record CreateItemAddonCommand(List<AddItemAddonCommandData> Addons) : IRequest<Result<List<ItemAddonResponse>>>;

    public sealed record AddItemAddonCommandData(Guid ItemId, int OldId, string Name, FileInput? Image, string SemanticImageName, int? RarityId) :
        IHasName,
        IHasSemanticImageName;
}
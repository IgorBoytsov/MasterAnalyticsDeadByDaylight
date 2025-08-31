using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.GetAll
{
    public sealed record GetAllItemsQuery() : IRequest<List<ItemSoloResponse>>;
}
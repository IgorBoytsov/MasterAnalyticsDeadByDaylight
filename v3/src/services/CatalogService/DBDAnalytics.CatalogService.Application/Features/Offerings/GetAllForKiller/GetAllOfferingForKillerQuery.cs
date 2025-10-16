using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAllForKiller
{
    public sealed record GetAllOfferingForKillerQuery() : IRequest<List<OfferingResponse>>;
}
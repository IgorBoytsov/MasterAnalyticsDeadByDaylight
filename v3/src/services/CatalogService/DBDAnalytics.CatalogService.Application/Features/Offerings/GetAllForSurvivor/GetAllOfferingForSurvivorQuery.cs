using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAllForSurvivor
{
    public sealed record GetAllOfferingForSurvivorQuery() : IRequest<List<OfferingResponse>>;
}
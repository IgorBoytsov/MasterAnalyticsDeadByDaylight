using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.GetAll
{
    public sealed record GetAllOfferingQuery() : IRequest<List<OfferingResponse>>;
}
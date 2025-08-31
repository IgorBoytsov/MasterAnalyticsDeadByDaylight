using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.GetAll
{
    public sealed record GetAllPlayerAssociationsQuery() : IRequest<List<PlayerAssociationResponse>>;
}
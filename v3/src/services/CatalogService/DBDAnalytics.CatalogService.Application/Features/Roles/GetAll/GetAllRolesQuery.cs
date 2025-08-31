using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.GetAll
{
    public sealed record GetAllRolesQuery() : IRequest<List<RoleResponse>>;
}
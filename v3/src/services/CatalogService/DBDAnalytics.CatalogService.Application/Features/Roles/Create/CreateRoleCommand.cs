using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Create
{
    public sealed record CreateRoleCommand(int OldId, string Name) : IRequest<Result<RoleResponse>>,
        IHasName;
}
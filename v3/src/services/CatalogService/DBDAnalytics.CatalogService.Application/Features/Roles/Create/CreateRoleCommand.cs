using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Create
{
    public sealed record CreateRoleCommand(int OldId, string Name) : IRequest<Result<RoleResponse>>,
        IHasName;
}
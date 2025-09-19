using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Update
{
    public sealed record UpdateRoleCommand(int RoleId, string Name) : IRequest<Result>,
        IHasName;
}
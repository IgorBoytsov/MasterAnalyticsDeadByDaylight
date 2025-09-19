using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Delete
{
    public sealed record DeleteRoleCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
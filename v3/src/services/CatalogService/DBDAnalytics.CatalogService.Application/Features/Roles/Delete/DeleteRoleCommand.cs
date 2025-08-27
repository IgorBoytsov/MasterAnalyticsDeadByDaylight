using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Delete
{
    public sealed record DeleteRoleCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Delete
{
    public sealed record DeletePlayerAssociationCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
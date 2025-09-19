using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Delete
{
    public sealed record DeletePlayerAssociationCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
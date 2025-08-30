using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Update
{
    public sealed record UpdatePlayerAssociationCommand(int PlayerAssociationID, string Name) : IRequest<Result>,
        IHasName;
} 
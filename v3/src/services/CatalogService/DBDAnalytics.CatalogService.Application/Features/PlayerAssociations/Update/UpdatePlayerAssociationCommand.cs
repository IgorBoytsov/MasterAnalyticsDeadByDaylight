using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Update
{
    public sealed record UpdatePlayerAssociationCommand(int PlayerAssociationID, string Name) : IRequest<Result>,
        IHasName;
} 
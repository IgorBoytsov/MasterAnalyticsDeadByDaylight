using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Create
{
    public sealed record CreatePlayerAssociationCommand(int OldId, string Name) : IRequest<Result<PlayerAssociationResponse>>,
        IHasName;
}
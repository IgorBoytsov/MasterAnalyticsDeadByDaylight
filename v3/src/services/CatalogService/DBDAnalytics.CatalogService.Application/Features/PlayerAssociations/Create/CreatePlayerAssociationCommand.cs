using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Create
{
    public sealed record CreatePlayerAssociationCommand(int OldId, string Name) : IRequest<Result<PlayerAssociationResponse>>,
        IHasName;
}
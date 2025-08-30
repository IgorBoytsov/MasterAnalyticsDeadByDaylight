using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using DBDAnalytics.Shared.Contracts.Requests.Shared;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Update
{
    public sealed record UpdateKillerCommand(Guid Id, string Name, 
        FileInput? ImageAbility, string SemanticImageAbilityName,
        FileInput? ImagePortrait, string SemanticImagePortraitName) : IRequest<Result>,
        IHasGuidId,
        IHasName;
}
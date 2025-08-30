using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Update
{
    public sealed class UpdatePlayerAssociationCommandValidator : AbstractValidator<UpdatePlayerAssociationCommand>
    {
        public UpdatePlayerAssociationCommandValidator() => Include(new NameValidator<UpdatePlayerAssociationCommand>(PlayerAssociationName.MAX_LENGTH));
    }
}
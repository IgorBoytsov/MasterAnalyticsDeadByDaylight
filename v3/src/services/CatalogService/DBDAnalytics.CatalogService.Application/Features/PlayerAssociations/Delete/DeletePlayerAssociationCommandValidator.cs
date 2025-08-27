using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Delete
{
    public sealed class DeletePlayerAssociationCommandValidator : AbstractValidator<DeletePlayerAssociationCommand>
    {
        public DeletePlayerAssociationCommandValidator() => Include(new IntValidator<DeletePlayerAssociationCommand>());
    }
}
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.UpdatePerk
{
    public sealed class UpdateKillerPerkCommandValidator : AbstractValidator<UpdateKillerPerkCommand>
    {
        public UpdateKillerPerkCommandValidator()
        {
            Include(new NameValidator<UpdateKillerPerkCommand>(KillerPerkName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateKillerPerkCommand>());
        }
    }
}
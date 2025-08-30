using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Update
{
    public sealed class UpdateOfferingCommandValidator : AbstractValidator<UpdateOfferingCommand>
    {
        public UpdateOfferingCommandValidator()
        {
            Include(new GuidValidator<UpdateOfferingCommand>());
            Include(new NameValidator<UpdateOfferingCommand>(OfferingName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateOfferingCommand>());
        }
    }
}
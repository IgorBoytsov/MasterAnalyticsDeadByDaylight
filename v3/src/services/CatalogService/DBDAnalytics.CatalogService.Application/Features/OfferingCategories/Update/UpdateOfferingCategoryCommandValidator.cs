using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Update
{
    public sealed class UpdateOfferingCategoryCommandValidator : AbstractValidator<UpdateOfferingCategoryCommand>
    {
        public UpdateOfferingCategoryCommandValidator() => Include(new NameValidator<UpdateOfferingCategoryCommand>(OfferingCategoryName.MAX_LENGTH));
    }
}
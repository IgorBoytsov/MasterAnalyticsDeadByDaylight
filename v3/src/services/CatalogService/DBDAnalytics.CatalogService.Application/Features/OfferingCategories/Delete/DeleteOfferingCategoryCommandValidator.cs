using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Delete
{
    public sealed class DeleteOfferingCategoryCommandValidator : AbstractValidator<DeleteOfferingCategoryCommand>
    {
        public DeleteOfferingCategoryCommandValidator() => Include(new IntValidator<DeleteOfferingCategoryCommand>());
    }
}
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignCategory
{
    public sealed class AssignCategoryToOfferingCommandValidator : AbstractValidator<AssignCategoryToOfferingCommand>
    {
        public AssignCategoryToOfferingCommandValidator()
        {
            Include(new MustCategoryIdValidator<AssignCategoryToOfferingCommand>());
        }
    }
}
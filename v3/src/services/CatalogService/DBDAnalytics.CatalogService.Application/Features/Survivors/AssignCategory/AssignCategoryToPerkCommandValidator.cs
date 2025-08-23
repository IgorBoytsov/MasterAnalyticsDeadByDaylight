using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.AssignCategory
{
    public sealed class AssignCategoryToPerkCommandValidator : AbstractValidator<AssignCategoryToPerkCommand>
    {
        public AssignCategoryToPerkCommandValidator()
        {
            Include(new PerkIdValidator<AssignCategoryToPerkCommand>());
            Include(new MustCategoryIdValidator<AssignCategoryToPerkCommand>());
        }
    }
}
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AssignCategoryToPerk
{
    public sealed class AssignCategoryToPerkCommandValidator : AbstractValidator<AssignCategoryToPerkCommand>
    {
        public AssignCategoryToPerkCommandValidator()
        {
            Include(new KillerIdValidator<AssignCategoryToPerkCommand>());
            Include(new PerkIdValidator<AssignCategoryToPerkCommand>());
            Include(new MustCategoryIdValidator<AssignCategoryToPerkCommand>());
        }
    }
}
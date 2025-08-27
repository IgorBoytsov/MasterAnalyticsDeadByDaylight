using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Delete
{
    public sealed class DeleteSurvivorPerkCategoryCommandValidator : AbstractValidator<DeleteSurvivorPerkCategoryCommand>
    {
        public DeleteSurvivorPerkCategoryCommandValidator() => Include(new IntValidator<DeleteSurvivorPerkCategoryCommand>());
    }
}
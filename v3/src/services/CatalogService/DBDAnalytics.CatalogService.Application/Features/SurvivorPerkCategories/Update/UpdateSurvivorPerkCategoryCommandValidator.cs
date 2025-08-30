using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Update
{
    public sealed class UpdateSurvivorPerkCategoryCommandValidator : AbstractValidator<UpdateSurvivorPerkCategoryCommand>
    {
        public UpdateSurvivorPerkCategoryCommandValidator() => Include(new NameValidator<UpdateSurvivorPerkCategoryCommand>(SurvivorPerkCategoryName.MAX_LENGTH));
    }
}
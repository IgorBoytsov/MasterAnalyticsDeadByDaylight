using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Update
{
    public sealed class UpdateKillerPerkCategoryCommandValidator : AbstractValidator<UpdateKillerPerkCategoryCommand>
    {
        public UpdateKillerPerkCategoryCommandValidator() => Include(new NameValidator<UpdateKillerPerkCategoryCommand>(KillerPerkCategoryName.MAX_LENGTH));
    }
}
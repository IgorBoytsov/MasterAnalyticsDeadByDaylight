using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create
{
    public sealed class CreateKillerPerkCategoryCommandValidator : AbstractValidator<CreateKillerPerkCategoryCommand>
    {
        public CreateKillerPerkCategoryCommandValidator()
        {
            Include(new NameValidator<CreateKillerPerkCategoryCommand>());
        }
    }
}
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Delete
{
    public sealed class DeleteKillerPerkCategoryCommandValidator : AbstractValidator<DeleteKillerPerkCategoryCommand>
    {
        public DeleteKillerPerkCategoryCommandValidator() => Include(new IntValidator<DeleteKillerPerkCategoryCommand>());
    }
}
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Create
{
    public sealed class CreateSurvivorPerkCategoryCommandValidator : AbstractValidator<CreateSurvivorPerkCategoryCommand>
    {
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepository;

        public CreateSurvivorPerkCategoryCommandValidator(ISurvivorPerkCategoryRepository survivorPerkCategoryRepository)
        {
            _survivorPerkCategoryRepository = survivorPerkCategoryRepository;

            Include(new NameValidator<CreateSurvivorPerkCategoryCommand>(SurvivorPerkCategoryName.MAX_LENGTH));

            When(spc => !string.IsNullOrWhiteSpace(spc.Name), () =>
            {
                RuleFor(spc => spc.Name)
                    .MustAsync(async (name, clt) => !await _survivorPerkCategoryRepository.Exist(name)).WithMessage("Категория с таким названием уже существует.");
            });
        }
    }
}
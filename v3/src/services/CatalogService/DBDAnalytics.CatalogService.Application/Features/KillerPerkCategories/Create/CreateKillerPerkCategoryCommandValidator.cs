using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create
{
    public sealed class CreateKillerPerkCategoryCommandValidator : AbstractValidator<CreateKillerPerkCategoryCommand>
    {
        private readonly IKillerPerkCategoryRepository _killerPerkCategoryRepository;

        public CreateKillerPerkCategoryCommandValidator(IKillerPerkCategoryRepository killerPerkCategoryRepository)
        {
            _killerPerkCategoryRepository = killerPerkCategoryRepository;

            Include(new NameValidator<CreateKillerPerkCategoryCommand>(KillerName.MAX_LENGTH));

            When(kpc => !string.IsNullOrWhiteSpace(kpc.Name), () =>
            {
                RuleFor(kpc => kpc.Name)
                    .MustAsync(async (name, clt) => !await _killerPerkCategoryRepository.Exist(name)).WithMessage("Категория с таким названием уже существует.");
            });
        }
    }
}
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Create
{
    public sealed class CreateRarityCommandValidator : AbstractValidator<CreateRarityCommand>
    {
        private readonly IRarityRepository _rarityRepository;

        public CreateRarityCommandValidator(IRarityRepository rarityRepository)
        {
            _rarityRepository = rarityRepository;

            Include(new NameValidator<CreateRarityCommand>(RarityName.MAX_LENGTH));

            When(r => !string.IsNullOrWhiteSpace(r.Name), () =>
            {
                RuleFor(r => r.Name)
                    .MustAsync(async (name, clt) => !await _rarityRepository.Exist(name)).WithMessage("Качество с таким названием уже существует.");
            });
        }
    }
}
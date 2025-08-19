using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create
{
    public sealed class CreateWhoPlacedMapCommandValidator : AbstractValidator<CreateWhoPlacedMapCommand>
    {
        private readonly IWhoPlacedMapRepository _whoPlacedMapRepository;

        public CreateWhoPlacedMapCommandValidator(IWhoPlacedMapRepository whoPlacedMapRepository)
        {
            _whoPlacedMapRepository = whoPlacedMapRepository;

            Include(new NameValidator<CreateWhoPlacedMapCommand>(PlacedMapName.MAX_LENGTH));

            When(w => !string.IsNullOrWhiteSpace(w.Name), () =>
            {
                RuleFor(w => w.Name)
                    .MustAsync(async (name, clt) => !await _whoPlacedMapRepository.Exist(name)).WithMessage($"Описание того, кто поставил карту уже существует."); ;
            });
        }
    }
}
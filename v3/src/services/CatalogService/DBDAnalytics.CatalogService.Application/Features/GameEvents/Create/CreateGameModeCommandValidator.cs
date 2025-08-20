using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Create
{
    public sealed class CreateGameModeCommandValidator : AbstractValidator<CreateGameModeCommand>
    {
        private readonly IGameModeRepository _gameModeRepository;

        public CreateGameModeCommandValidator(IGameModeRepository gameModeRepository)
        {
            _gameModeRepository = gameModeRepository;

            Include(new NameValidator<CreateGameModeCommand>(GameModeName.MAX_LENGTH));

            When(gm => !string.IsNullOrWhiteSpace(gm.Name), () =>
            {
                RuleFor(gm => gm.Name)
                    .MustAsync(async (name, clt) => !await _gameModeRepository.Exist(name)).WithMessage("Игровой режим с таким названием уже существует.");
            });
        }
    }
}
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Create
{
    public sealed class CreateGameEventCommandValidator : AbstractValidator<CreateGameEventCommand>
    {
        private readonly IGameEventRepository _gameEventRepository;

        public CreateGameEventCommandValidator(IGameEventRepository gameEventRepository)
        {
            _gameEventRepository = gameEventRepository;

            Include(new NameValidator<CreateGameEventCommand>(GameEventName.MAX_LENGTH));

            When(ge => !string.IsNullOrWhiteSpace(ge.Name), () =>
            {
                RuleFor(ge => ge.Name)
                    .MustAsync(async (name, clt) => !await _gameEventRepository.Exist(name)).WithMessage("Игровой ивент с таким названием уже существует.");
            });
        }
    }
}
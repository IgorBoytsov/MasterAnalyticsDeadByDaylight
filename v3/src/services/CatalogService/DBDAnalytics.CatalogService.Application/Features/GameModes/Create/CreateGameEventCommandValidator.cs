using DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Create
{
    public sealed class CreateGameEventCommandValidator : AbstractValidator<CreateGameEventCommand>
    {
        public CreateGameEventCommandValidator()
        {
            Include(new NameValidator<CreateGameEventCommand>(GameEventName.MAX_LENGTH));
        }
    }
}
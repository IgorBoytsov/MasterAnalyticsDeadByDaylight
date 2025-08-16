using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Create
{
    public sealed class CreateGameModeCommandValidator : AbstractValidator<CreateGameModeCommand>
    {
        public CreateGameModeCommandValidator()
        {
            Include(new NameValidator<CreateGameModeCommand>(GameModeName.MAX_LENGTH));
        }
    }
}
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Delete
{
    public sealed class DeleteGameModeCommandValidator : AbstractValidator<DeleteGameModeCommand>
    {
        public DeleteGameModeCommandValidator() => Include(new IntValidator<DeleteGameModeCommand>());
    }
}
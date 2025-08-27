using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete
{
    public sealed class DeleteGameEventCommandValidator : AbstractValidator<DeleteGameEventCommand>
    {
        public DeleteGameEventCommandValidator() => Include(new IntValidator<DeleteGameEventCommand>());
    }
}
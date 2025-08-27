using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Delete
{
    public sealed class DeleteRarityCommandValidator : AbstractValidator<DeleteRarityCommand>
    {
        public DeleteRarityCommandValidator() => Include(new IntValidator<DeleteRarityCommand>());
    }
}
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Delete
{
    public sealed class DeleteSurvivorCommandValidator : AbstractValidator<DeleteSurvivorCommand>
    {
        public DeleteSurvivorCommandValidator() => Include(new GuidValidator<DeleteSurvivorCommand>());
    }
}
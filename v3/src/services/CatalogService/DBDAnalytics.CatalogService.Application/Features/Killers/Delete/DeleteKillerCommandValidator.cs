using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Delete
{
    public sealed class DeleteKillerCommandValidator : AbstractValidator<DeleteKillerCommand>
    {
        public DeleteKillerCommandValidator() => Include(new GuidValidator<DeleteKillerCommand>());
    }
}
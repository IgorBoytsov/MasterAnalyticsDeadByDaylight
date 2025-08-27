using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Delete
{
    public sealed class DeleteTypeDeathCommandValidator : AbstractValidator<DeleteTypeDeathCommand>
    {
        public DeleteTypeDeathCommandValidator() => Include(new IntValidator<DeleteTypeDeathCommand>());
    }
}
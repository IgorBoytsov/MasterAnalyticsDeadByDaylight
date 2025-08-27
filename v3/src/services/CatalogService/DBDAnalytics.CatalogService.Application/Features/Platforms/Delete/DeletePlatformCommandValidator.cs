using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Delete
{
    public sealed class DeletePlatformCommandValidator : AbstractValidator<DeletePlatformCommand>
    {
        public DeletePlatformCommandValidator() => Include(new IntValidator<DeletePlatformCommand>());
    }
}
using DBDAnalytics.CatalogService.Domain.ValueObjects.Platform;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Update
{
    public sealed class UpdatePlatformCommandValidator : AbstractValidator<UpdatePlatformCommand>
    {
        public UpdatePlatformCommandValidator() => Include(new NameValidator<UpdatePlatformCommand>(PlatformName.MAX_LENGTH));
    }
}
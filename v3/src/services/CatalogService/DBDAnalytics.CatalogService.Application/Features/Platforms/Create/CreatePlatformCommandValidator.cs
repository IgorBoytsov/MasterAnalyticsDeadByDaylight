using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Platform;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Create
{
    public sealed class CreatePlatformCommandValidator : AbstractValidator<CreatePlatformCommand>
    {
        private readonly IPlatformRepository _platformRepository;

        public CreatePlatformCommandValidator(IPlatformRepository platformRepository)
        {
            _platformRepository = platformRepository;

            Include(new NameValidator<CreatePlatformCommand>(PlatformName.MAX_LENGTH));

            When(p => !string.IsNullOrWhiteSpace(p.Name), () =>
            {
                RuleFor(p => p.Name)
                    .MustAsync(async (name, clt) => !await _platformRepository.Exist(name)).WithMessage("Платформа с таким названием уже существует.");
            });
        }
    }
}
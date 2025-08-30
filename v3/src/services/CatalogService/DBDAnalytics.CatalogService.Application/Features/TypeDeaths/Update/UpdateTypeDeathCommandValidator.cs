using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Update
{
    public sealed class UpdateTypeDeathCommandValidator : AbstractValidator<UpdateTypeDeathCommand>
    {
        public UpdateTypeDeathCommandValidator() => Include(new NameValidator<UpdateTypeDeathCommand>(TypeDeathName.MAX_LENGTH));
    }
}
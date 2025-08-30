using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Update
{
    public sealed class UpdateKillerCommandValidator : AbstractValidator<UpdateKillerCommand>
    {
        public UpdateKillerCommandValidator()
        {
            Include(new GuidValidator<UpdateKillerCommand>());
            Include(new NameValidator<UpdateKillerCommand>(KillerName.MAX_LENGTH));
        }
    }
}
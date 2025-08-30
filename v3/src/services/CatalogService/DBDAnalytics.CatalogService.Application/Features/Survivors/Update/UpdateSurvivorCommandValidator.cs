using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Update
{
    public sealed class UpdateSurvivorCommandValidator : AbstractValidator<UpdateSurvivorCommand>
    {
        public UpdateSurvivorCommandValidator()
        {
            Include(new NameValidator<UpdateSurvivorCommand>(SurvivorName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateSurvivorCommand>());
        }
    }
}
using DBDAnalytics.CatalogService.Application.Features.Killers.AddPerk;
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.AddPerk
{
    public sealed class CreateSurvivorPerkCommandValidator : AbstractValidator<CreateSurvivorPerkCommand>
    {
        public CreateSurvivorPerkCommandValidator()
        {
            Include(new PerksValidator<CreateSurvivorPerkCommand, AddSurvivorPerkCommandData>(new CreateSurvivorPerkCommandDataValidator(), mustNotBeEmpty: true));;
        }
    }

    public sealed class CreateSurvivorPerkCommandDataValidator : AbstractValidator<AddSurvivorPerkCommandData>
    {
        public CreateSurvivorPerkCommandDataValidator()
        {
            Include(new NameValidator<AddSurvivorPerkCommandData>(SurvivorPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddSurvivorPerkCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}
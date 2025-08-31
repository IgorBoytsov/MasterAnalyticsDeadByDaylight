using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Create
{
    public sealed class CreateKillerPerkCommandValidator : AbstractValidator<CreateKillerPerkCommand>
    {
        public CreateKillerPerkCommandValidator()
        {
            Include(new PerksValidator<CreateKillerPerkCommand, AddPerkToKillerCommandData>(new AddPerkToKillerCommandDataValidator(), mustNotBeEmpty: true));
        }
    }

    public sealed class AddPerkToKillerCommandDataValidator : AbstractValidator<AddPerkToKillerCommandData>
    {
        public AddPerkToKillerCommandDataValidator()
        {
            Include(new KillerIdValidator<AddPerkToKillerCommandData>());
            Include(new NameValidator<AddPerkToKillerCommandData>(KillerPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddPerkToKillerCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}
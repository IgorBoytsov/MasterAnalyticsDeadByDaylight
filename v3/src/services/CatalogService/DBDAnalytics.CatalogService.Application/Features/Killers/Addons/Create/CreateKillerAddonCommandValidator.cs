using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Create
{
    public sealed class CreateKillerAddonCommandValidator : AbstractValidator<CreateKillerAddonCommand>
    {
        public CreateKillerAddonCommandValidator()
        {
            Include(new AddonsValidator<CreateKillerAddonCommand, AddAddonToKillerCommandData>(new AddAddonToKillerCommandDataValidator(), mustNotBeEmpty: true));
        }
    }

    public sealed class AddAddonToKillerCommandDataValidator : AbstractValidator<AddAddonToKillerCommandData>
    {
        public AddAddonToKillerCommandDataValidator()
        {
            Include(new KillerIdValidator<AddAddonToKillerCommandData>());
            Include(new NameValidator<AddAddonToKillerCommandData>(KillerAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddAddonToKillerCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Create
{
    public sealed class CreateItemAddonCommandValidator : AbstractValidator<CreateItemAddonCommand>
    {
        public CreateItemAddonCommandValidator()
        {
            Include(new AddonsValidator<CreateItemAddonCommand, AddItemAddonCommandData>(new CreateItemAddonCommandDataValidator(), mustNotBeEmpty: false));
        }
    }

    public sealed class CreateItemAddonCommandDataValidator : AbstractValidator<AddItemAddonCommandData>
    {
        public CreateItemAddonCommandDataValidator()
        {
            Include(new NameValidator<AddItemAddonCommandData>(ItemAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddItemAddonCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}
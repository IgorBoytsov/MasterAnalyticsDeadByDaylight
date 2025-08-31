using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Update
{
    public sealed class UpdateKillerAddonCommandValidator : AbstractValidator<UpdateKillerAddonCommand>
    {
        public UpdateKillerAddonCommandValidator()
        {
            Include(new NameValidator<UpdateKillerAddonCommand>(KillerAddonName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateKillerAddonCommand>());
        }
    }
}
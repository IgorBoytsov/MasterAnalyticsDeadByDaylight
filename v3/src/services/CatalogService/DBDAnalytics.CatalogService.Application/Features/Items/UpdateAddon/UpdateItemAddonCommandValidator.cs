using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.UpdateAddon
{
    public sealed class UpdateItemAddonCommandValidator : AbstractValidator<UpdateItemAddonCommand>
    {
        public UpdateItemAddonCommandValidator()
        {
            Include(new GuidValidator<UpdateItemAddonCommand>());
            Include(new NameValidator<UpdateItemAddonCommand>(ItemAddonName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateItemAddonCommand>());
        }
    }
}
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Update
{
    public sealed class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            Include(new GuidValidator<UpdateItemCommand>());
            Include(new NameValidator<UpdateItemCommand>(ItemName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateItemCommand>());
        }
    }
}
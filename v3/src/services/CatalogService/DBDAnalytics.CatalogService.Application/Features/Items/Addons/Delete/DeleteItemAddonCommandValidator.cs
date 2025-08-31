using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Delete
{
    public sealed class DeleteItemAddonCommandValidator : AbstractValidator<DeleteItemAddonCommand>
    {
        public DeleteItemAddonCommandValidator() 
        {
            RuleFor(x => x.IdItem)
                .NotEmpty().WithMessage("Идентификатор предмета не может быть пустым.");

            RuleFor(x => x.IdAddon)
                .NotEmpty().WithMessage("Идентификатор улучшения не может быть пустым.");
        }
    }
}
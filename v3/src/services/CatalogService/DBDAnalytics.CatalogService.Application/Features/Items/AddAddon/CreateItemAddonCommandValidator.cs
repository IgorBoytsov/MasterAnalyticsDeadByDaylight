using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.AddAddon
{
    public sealed class CreateItemAddonCommandValidator : AbstractValidator<CreateItemAddonCommand>
    {
        public CreateItemAddonCommandValidator()
        {
            RuleFor(x => x.Addons)
                .NotEmpty().WithMessage("Кол-во улучшений для предмета должно быть больше 0, что бы операция могла быть выполнена");

            RuleForEach(x => x.Addons)
                .SetValidator(new CreateItemAddonCommandDataValidator());

            RuleFor(x => x.Addons)
                .Must(addons => addons.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты аддонов по имени в одном запросе.")
                .When(x => x.Addons != null && x.Addons.Any());
        }
    }

    public sealed class CreateItemAddonCommandDataValidator : AbstractValidator<AddItemAddonCommandData>
    {
        public CreateItemAddonCommandDataValidator()
        {
            Include(new NameValidator<AddItemAddonCommandData>(ItemAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddItemAddonCommandData>());

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                .NotEmpty().WithMessage("Имя файла не может быть пустым");

                RuleFor(x => x.Image!.Content)
                .NotEmpty().WithMessage("Содержимое файла не может быть пустым.");
            });
        }
    }
}
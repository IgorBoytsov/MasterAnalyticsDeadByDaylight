using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AddAddon
{
    public sealed class CreateKillerAddonCommandValidator : AbstractValidator<CreateKillerAddonCommand>
    {
        public CreateKillerAddonCommandValidator()
        {
            RuleFor(x => x.Addons)
                .NotEmpty().WithMessage("Кол-во улучшений для киллера должно быть больше 0, что бы операция могла быть выполнена");

            RuleForEach(x => x.Addons)
                .SetValidator(new AddAddonToKillerCommandDataValidator());

            RuleFor(x => x.Addons)
                .Must(addons => addons.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты аддонов по имени в одном запросе.")
                .When(x => x.Addons != null && x.Addons.Any());
        }
    }

    public sealed class AddAddonToKillerCommandDataValidator : AbstractValidator<AddAddonToKillerCommandData>
    {
        public AddAddonToKillerCommandDataValidator()
        {
            Include(new KillerIdValidator<AddAddonToKillerCommandData>());
            Include(new NameValidator<AddAddonToKillerCommandData>(KillerAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddAddonToKillerCommandData>());

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
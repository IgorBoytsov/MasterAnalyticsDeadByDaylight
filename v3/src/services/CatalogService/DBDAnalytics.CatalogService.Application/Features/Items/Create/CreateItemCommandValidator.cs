using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Create
{
    public sealed class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        private readonly IItemRepository _itemRepository;

        public CreateItemCommandValidator(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;

            Include(new NameValidator<CreateItemCommand>(ItemName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateItemCommand>());

            RuleFor(x => x.Addons)
                .Must(perks => perks.GroupBy(p => p.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты улучшений по имени в одном запросе.")
                .When(x => x.Addons != null && x.Addons.Any());

            RuleForEach(x => x.Addons)
                .SetValidator(new CreateItemAddonCommandDataValidator());

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                .NotEmpty().WithMessage("Имя файла перка не может быть пустым");

                RuleFor(x => x.Image!.Content)
                .NotEmpty().WithMessage("Содержимое файла перка не может быть пустым.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(k => k.Name)
                    .MustAsync(async (name, cancellationToken) => !await _itemRepository.Exist(name)).WithMessage($"Предмет с таким именем уже существует");
            });
        }
    }

    public sealed class CreateItemAddonCommandDataValidator : AbstractValidator<CreateItemAddonCommandData>
    {
        public CreateItemAddonCommandDataValidator()
        {
            Include(new NameValidator<CreateItemAddonCommandData>(ItemAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateItemAddonCommandData>());

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                .NotEmpty().WithMessage("Имя файла перка не может быть пустым");

                RuleFor(x => x.Image!.Content)
                .NotEmpty().WithMessage("Содержимое файла перка не может быть пустым.");
            });
        }
    }
}
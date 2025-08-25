using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
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
            Include(new MayFileInputValidator());
            Include(new AddonsValidator<CreateItemCommand, CreateItemAddonCommandData>(new CreateItemAddonCommandDataValidator()));

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
            Include(new MayFileInputValidator());
        }
    }
}
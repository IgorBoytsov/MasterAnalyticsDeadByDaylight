using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AddPerk
{
    public sealed class CreateKillerPerkCommandValidator : AbstractValidator<CreateKillerPerkCommand>
    {
        public CreateKillerPerkCommandValidator()
        {
            RuleFor(x => x.Perks)
                .NotEmpty().WithMessage("Кол-во перков для киллера должно быть больше 0, что бы операция могла быть выполнена");

            RuleForEach(x => x.Perks)
                .SetValidator(new AddPerkToKillerCommandDataValidator());

            RuleFor(x => x.Perks)
                .Must(perks => perks.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты перков по имени в одном запросе.")
                .When(x => x.Perks != null && x.Perks.Any());
        }
    }

    public sealed class AddPerkToKillerCommandDataValidator : AbstractValidator<AddPerkToKillerCommandData>
    {
        public AddPerkToKillerCommandDataValidator()
        {
            Include(new KillerIdValidator<AddPerkToKillerCommandData>());
            Include(new NameValidator<AddPerkToKillerCommandData>(KillerPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddPerkToKillerCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}
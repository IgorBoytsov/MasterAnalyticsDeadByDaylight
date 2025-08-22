using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create
{
    public sealed class CreateOfferingCategoryCommandValidator : AbstractValidator<CreateOfferingCategoryCommand>
    {
        private readonly IOfferingCategoryRepository _offeringCategoryRepository;

        public CreateOfferingCategoryCommandValidator(IOfferingCategoryRepository offeringCategoryRepository)
        {
            _offeringCategoryRepository = offeringCategoryRepository;

            Include(new NameValidator<CreateOfferingCategoryCommand>(OfferingCategoryName.MAX_LENGTH));

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(c => c.Name)
                    .MustAsync(async (name, clt) => !await _offeringCategoryRepository.Exist(name));
            });
        }
    }
}
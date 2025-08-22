using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Create
{
    public sealed class CreateOfferingCommandValidator : AbstractValidator<CreateOfferingCommand>
    {
        private readonly IOfferingRepository _offeringRepository;

        public CreateOfferingCommandValidator(IOfferingRepository offeringRepository)
        {
            _offeringRepository = offeringRepository;

            Include(new NameValidator<CreateOfferingCommand>(OfferingName.MAX_LENGTH));
            Include(new RoleIdValidator());
            Include(new MayCategoryIdValidator<CreateOfferingCommand>());
            Include(new SemanticImageNameValidator<CreateOfferingCommand>());

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(o => o.Name)
                    .MustAsync(async (name, clt) => !await _offeringRepository.Exist(name));
            });
        }
    }
}
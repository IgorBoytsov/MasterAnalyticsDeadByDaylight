using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.PlayerAssociations.Create
{
    public sealed class CreatePlayerAssociationCommandValidator : AbstractValidator<CreatePlayerAssociationCommand>
    {
        private readonly IPlayerAssociationRepository _associationRepository;

        public CreatePlayerAssociationCommandValidator(IPlayerAssociationRepository playerAssociationRepository)
        {
            _associationRepository = playerAssociationRepository;

            Include(new NameValidator<CreatePlayerAssociationCommand>(PlayerAssociationName.MAX_LENGTH));

            When(pa => !string.IsNullOrWhiteSpace(pa.Name), () =>
            {
                RuleFor(pa => pa.Name)
                    .MustAsync(async (name, clt) => !await _associationRepository.Exist(name)).WithMessage("Игровая ассоциация с таким названием уже существует.");
            });
        }
    }
}
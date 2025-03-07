using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.AssociationCase
{
    public class CreateAssociationUseCase(IAssociationRepository associationRepository) : ICreateAssociationUseCase
    {
        private readonly IAssociationRepository _associationRepository = associationRepository;

        public async Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> CreateAsync(string playerAssociationName, string playerAssociationDescription)
        {
            string message = string.Empty;

            var (CreatedPlayerAssociationDomain, Message) = PlayerAssociationDomain.Create(0, playerAssociationName, playerAssociationDescription);

            if (CreatedPlayerAssociationDomain is null)
                return (null, Message);

            bool exist = await _associationRepository.ExistAsync(playerAssociationName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _associationRepository.CreateAsync(playerAssociationName, playerAssociationDescription);

            var domainEntity = await _associationRepository.GetAsync(id);

            if (domainEntity is null)
                return (null, "Запись не найдена.");

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
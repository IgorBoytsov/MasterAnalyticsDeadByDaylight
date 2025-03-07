using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.AssociationCase
{
    public class UpdateAssociationUseCase(IAssociationRepository associationRepository) : IUpdateAssociationUseCase
    {
        private readonly IAssociationRepository _associationRepository = associationRepository;

        public async Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription)
        {
            string message = string.Empty;

            if (idPlayerAssociation == 0 || string.IsNullOrWhiteSpace(playerAssociationName))
                return (null, "Такой записи не существует");

            var exist = await _associationRepository.ExistAsync(playerAssociationName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _associationRepository.UpdateAsync(idPlayerAssociation, playerAssociationName, playerAssociationDescription);

            var domainEntity = await _associationRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<PlayerAssociationDTO?> ForcedUpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription)
        {
            string message = string.Empty;

            int id = await _associationRepository.UpdateAsync(idPlayerAssociation, playerAssociationName, playerAssociationDescription);

            var domainEntity = await _associationRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
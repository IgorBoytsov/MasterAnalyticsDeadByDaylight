using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.AssociationCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.AssociationCase
{
    public class GetAssociationUseCase(IAssociationRepository associationRepository) : IGetAssociationUseCase
    {
        private readonly IAssociationRepository _associationRepository = associationRepository;

        public async Task<List<PlayerAssociationDTO>> GetAllAsync()
        {
            var domainEntities = await _associationRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<PlayerAssociationDTO> GetAll()
        {
            var domainEntities = _associationRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<PlayerAssociationDTO?> GetAsync(int idPlayerAssociation)
        {
            var domainEntity = await _associationRepository.GetAsync(idPlayerAssociation);

            if (domainEntity == null)
            {
                Debug.WriteLine($"PlayerAssociation с ID {idPlayerAssociation} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
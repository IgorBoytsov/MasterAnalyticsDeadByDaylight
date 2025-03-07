using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.PatchCase
{
    public class GetPatchUseCase(IPatchRepository patchRepository) : IGetPatchUseCase
    {
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<List<PatchDTO>> GetAllAsync()
        {
            var domainEntities = await _patchRepository.GetAllAsync();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public List<PatchDTO> GetAll()
        {
            var domainEntities = _patchRepository.GetAll();

            var dtoEntities = domainEntities.ToDTO();

            return dtoEntities;
        }

        public async Task<PatchDTO?> GetAsync(int idPatch)
        {
            var domainEntity = await _patchRepository.GetAsync(idPatch);

            if (domainEntity == null)
            {
                Debug.WriteLine($"Patch с ID {idPatch} не найден в репозитории.");
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}

using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PatchCase
{
    public class UpdatePatchUseCase(IPatchRepository patchRepository) : IUpdatePatchUseCase
    {
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<(PatchDTO? PatchDTO, string? Message)> UpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            string message = string.Empty;

            if (idPatch == 0 || string.IsNullOrWhiteSpace(patchNumber))
                return (null, "Такой записи не существует");

            var exist = await _patchRepository.ExistAsync(patchNumber);

            if (exist)
                return (null, "Номер на который вы хотите поменять - уже существует.");

            int id = await _patchRepository.UpdateAsync(idPatch, patchNumber, patchDateRelease);

            var domainEntity = await _patchRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<PatchDTO?> ForcedUpdateAsync(int idPatch, string patchNumber, DateOnly patchDateRelease)
        {
            int id = await _patchRepository.UpdateAsync(idPatch, patchNumber, patchDateRelease);

            var domainEntity = await _patchRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
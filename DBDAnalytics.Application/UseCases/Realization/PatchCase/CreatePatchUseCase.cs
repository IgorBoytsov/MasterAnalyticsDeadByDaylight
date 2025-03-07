using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.PatchCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.PatchCase
{
    public class CreatePatchUseCase(IPatchRepository patchRepository) : ICreatePatchUseCase
    {
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<(PatchDTO? PatchDTO, string? Message)> CreateAsync(string patchNumber, DateOnly patchDateRelease)
        {
            string message = string.Empty;

            var (CreatedPatch, Message) = PatchDomain.Create(0, patchNumber, patchDateRelease);

            if (CreatedPatch is null)
            {
                return (null, Message);
            }

            bool exist = await _patchRepository.ExistAsync(patchNumber);

            if (exist)
                return (null, "Патч с таким номером уже существует.");

            var id = await _patchRepository.CreateAsync(patchNumber, patchDateRelease);

            var domainEntity = await _patchRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
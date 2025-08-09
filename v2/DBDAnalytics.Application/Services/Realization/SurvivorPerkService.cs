using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class SurvivorPerkService(ICreateSurvivorPerkUseCase createSurvivorPerkUseCase,
                                     IDeleteSurvivorPerkUseCase deleteSurvivorPerkUseCase,
                                     IGetSurvivorPerkUseCase getSurvivorPerkUseCase,
                                     IUpdateSurvivorPerkUseCase updateSurvivorPerkUseCase) : ISurvivorPerkService
    {
        private readonly ICreateSurvivorPerkUseCase _createSurvivorPerkUseCase = createSurvivorPerkUseCase;
        private readonly IDeleteSurvivorPerkUseCase _deleteSurvivorPerkUseCase = deleteSurvivorPerkUseCase;
        private readonly IGetSurvivorPerkUseCase _getSurvivorPerkUseCase = getSurvivorPerkUseCase;
        private readonly IUpdateSurvivorPerkUseCase _updateSurvivorPerkUseCase = updateSurvivorPerkUseCase;

        public async Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> CreateAsync(int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            return await _createSurvivorPerkUseCase.CreateAsync(idSurvivor, perkName, perkImage, idCategory, perkDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk)
        {
            return await _deleteSurvivorPerkUseCase.DeleteAsync(idPerk);
        }

        public List<SurvivorPerkDTO> GetAll()
        {
            return _getSurvivorPerkUseCase.GetAll();
        }

        public async Task<List<SurvivorPerkDTO>> GetAllAsync()
        {
            return await _getSurvivorPerkUseCase.GetAllAsync();
        }

        public async Task<SurvivorPerkDTO> GetAsync(int idPerk)
        {
            return await _getSurvivorPerkUseCase.GetAsync(idPerk);
        }

        public async Task<(SurvivorPerkDTO? SurvivorPerkDTO, string? Message)> UpdateAsync(int idPerk, int idSurvivor, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            return await _updateSurvivorPerkUseCase.UpdateAsync(idPerk, idSurvivor, perkName, perkImage, idCategory, perkDescription);
        }
    }
}
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class SurvivorService(ICreateSurvivorUseCase createSurvivorUseCase,
                                 IDeleteSurvivorUseCase deleteSurvivorUseCase,
                                 IGetSurvivorUseCase getSurvivorUseCase,
                                 IGetSurvivorWithPerksUseCase geSurvivorWithPerksUseCase,
                                 IUpdateSurvivorUseCase updateSurvivorUseCase) : ISurvivorService
    {
        private readonly ICreateSurvivorUseCase _createSurvivorUseCase = createSurvivorUseCase;
        private readonly IDeleteSurvivorUseCase _deleteSurvivorUseCase = deleteSurvivorUseCase;
        private readonly IGetSurvivorUseCase _getSurvivorUseCase = getSurvivorUseCase;
        private readonly IGetSurvivorWithPerksUseCase _getSurvivorWithPerksUseCase = geSurvivorWithPerksUseCase;
        private readonly IUpdateSurvivorUseCase _updateSurvivorUseCase = updateSurvivorUseCase;

        public async Task<(SurvivorDTO? SurvivorDTO, string Message)> CreateAsync(string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            return await _createSurvivorUseCase.CreateAsync(survivorName, survivorImage, survivorDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idSurvivor)
        {
            return await _deleteSurvivorUseCase.DeleteAsync(idSurvivor);
        }

        public List<SurvivorDTO> GetAll()
        {
            return _getSurvivorUseCase.GetAll();
        }

        public async Task<List<SurvivorDTO>> GetAllAsync()
        {
            return await _getSurvivorUseCase.GetAllAsync();
        }

        public async Task<SurvivorDTO> GetAsync(int idSurvivor)
        {
            return await _getSurvivorUseCase.GetAsync(idSurvivor);
        }

        public async Task<List<SurvivorWithPerksDTO>> GetSurvivorsWithPerksAsync()
        {
            return await _getSurvivorWithPerksUseCase.GetSurvivorsWithPerksAsync();
        }

        public async Task<SurvivorWithPerksDTO> GetSurvivorWithPerksAsync(int idSurvivor)
        {
            return await _getSurvivorWithPerksUseCase.GetSurvivorWithPerksAsync(idSurvivor);
        }

        public async Task<(SurvivorDTO? SurvivorDTO, string? Message)> UpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            return await _updateSurvivorUseCase.UpdateAsync(idSurvivor, survivorName, survivorImage, survivorDescription);
        }

        public async Task<SurvivorDTO> ForcedUpdateAsync(int idSurvivor, string survivorName, byte[]? survivorImage, string? survivorDescription)
        {
            return await _updateSurvivorUseCase.ForcedUpdateAsync(idSurvivor, survivorName, survivorImage, survivorDescription);
        }
    }
}
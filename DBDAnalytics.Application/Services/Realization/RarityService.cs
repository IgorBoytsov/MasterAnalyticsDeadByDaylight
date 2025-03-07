using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.RarityCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class RarityService(ICreateRarityUseCase createRarityMapUseCase,
                               IDeleteRarityUseCase deleteRarityUseCase,
                               IGetRarityUseCase getRarityUseCase,
                               IUpdateRarityUseCase updateRarityUseCase) : IRarityService
    {
        private readonly ICreateRarityUseCase _createRarityUseCase = createRarityMapUseCase;
        private readonly IDeleteRarityUseCase _deleteRarityUseCase = deleteRarityUseCase;
        private readonly IGetRarityUseCase _getRarityUseCase = getRarityUseCase;
        private readonly IUpdateRarityUseCase _updateRarityUseCase = updateRarityUseCase;

        public async Task<(RarityDTO? RarityDTO, string Message)> CreateAsync(string rarityName)
        {
            return await _createRarityUseCase.CreateAsync(rarityName);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idRarity)
        {
            return await _deleteRarityUseCase.DeleteAsync(idRarity);
        }

        public List<RarityDTO> GetAll()
        {
            return _getRarityUseCase.GetAll();
        }

        public async Task<List<RarityDTO>> GetAllAsync()
        {
            return await _getRarityUseCase.GetAllAsync();
        }

        public async Task<RarityDTO> GetAsync(int idRarity)
        {
            return await _getRarityUseCase.GetAsync(idRarity);
        }

        public async Task<(RarityDTO? RarityDTO, string? Message)> UpdateAsync(int idRarity, string rarityName)
        {
            return await _updateRarityUseCase.UpdateAsync(idRarity, rarityName);
        }
    }
}

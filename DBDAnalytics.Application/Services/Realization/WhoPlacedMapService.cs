using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class WhoPlacedMapService(ICreateWhoPlacedMapUseCase createWhoPlacedMapUseCase,
                                     IDeleteWhoPlacedMapUseCase deleteWhoPlacedMapUseCase,
                                     IGetWhoPlacedMapUseCase getWhoPlacedMapUseCase,
                                     IUpdateWhoPlacedMapUseCase updateWhoPlacedMapUseCase) : IWhoPlacedMapService
    {
        private readonly ICreateWhoPlacedMapUseCase _createWhoPlacedMapUseCase = createWhoPlacedMapUseCase;
        private readonly IDeleteWhoPlacedMapUseCase _deleteWhoPlacedMapUseCase = deleteWhoPlacedMapUseCase;
        private readonly IGetWhoPlacedMapUseCase _geWhoPlacedMapUseCase = getWhoPlacedMapUseCase;
        private readonly IUpdateWhoPlacedMapUseCase _updateWhoPlacedMapUseCase = updateWhoPlacedMapUseCase;

        public async Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string Message)> CreateAsync(string whoPlacedMapName)
        {
            return await _createWhoPlacedMapUseCase.CreateAsync(whoPlacedMapName);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idWhoPlacedMap)
        {
            return await _deleteWhoPlacedMapUseCase.DeleteAsync(idWhoPlacedMap);
        }

        public List<WhoPlacedMapDTO> GetAll()
        {
            return _geWhoPlacedMapUseCase.GetAll();
        }

        public async Task<List<WhoPlacedMapDTO>> GetAllAsync()
        {
            return await _geWhoPlacedMapUseCase.GetAllAsync();
        }

        public async Task<WhoPlacedMapDTO> GetAsync(int idWhoPlacedMap)
        {
            return await _geWhoPlacedMapUseCase.GetAsync(idWhoPlacedMap);
        }

        public async Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName)
        {
            return await _updateWhoPlacedMapUseCase.UpdateAsync(idWhoPlacedMap, whoPlacedMapName);
        }
    }
}

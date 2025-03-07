using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.MapCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class MapService(ICreateMapUseCase createMapUseCase,
                            IDeleteMapUseCase deleteMapUseCase,
                            IGetMapUseCase getMapUseCase,
                            IUpdateMapUseCase updateMapUseCase) : IMapService
    {
        private readonly ICreateMapUseCase _createMapUseCase = createMapUseCase;
        private readonly IDeleteMapUseCase _deleteMapUseCase = deleteMapUseCase;
        private readonly IGetMapUseCase _getMapUseCase = getMapUseCase;
        private readonly IUpdateMapUseCase _updateMapUseCase = updateMapUseCase;

        public async Task<(MapDTO? MapDTO, string? Message)> CreateAsync(int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            return await _createMapUseCase.CreateAsync(idMeasurement, mapName, mapImage, mapDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMap)
        {
            return await _deleteMapUseCase.DeleteAsync(idMap);
        }

        public List<MapDTO> GetAll()
        {
            return _getMapUseCase.GetAll();
        }

        public async Task<List<MapDTO>> GetAllAsync()
        {
            return await _getMapUseCase.GetAllAsync();
        }

        public async Task<MapDTO> GetAsync(int idMap)
        {
            return await _getMapUseCase.GetAsync(idMap);
        }

        public async Task<(MapDTO? MapDTO, string? Message)> UpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            return await _updateMapUseCase.UpdateAsync(idMap, idMeasurement, mapName, mapImage, mapDescription);
        }

        public async Task<MapDTO> ForcedUpdateAsync(int idMap, int idMeasurement, string mapName, byte[] mapImage, string mapDescription)
        {
            return await _updateMapUseCase.ForcedUpdateAsync(idMap, idMeasurement, mapName, mapImage, mapDescription);
        }
    }
}
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.PlatformCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class PlatformService(ICreatePlatformUseCase createPlatformUseCase,
                                 IDeletePlatformUseCase deletePlatformUseCase,
                                 IGetPlatformUseCase getPlatformUseCase,
                                 IUpdatePlatformUseCase updatePlatformUseCase) : IPlatformService
    {
        private readonly ICreatePlatformUseCase _createPlatformUseCase = createPlatformUseCase;
        private readonly IDeletePlatformUseCase _deletePlatformUseCase = deletePlatformUseCase;
        private readonly IGetPlatformUseCase _getPlatformUseCase = getPlatformUseCase;
        private readonly IUpdatePlatformUseCase _updatePlatformUseCase = updatePlatformUseCase;

        public async Task<(PlatformDTO? PlatformDTO, string Message)> CreateAsync(string platformName, string platformDescription)
        {
            return await _createPlatformUseCase.CreateAsync(platformName, platformDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlatform)
        {
            return await _deletePlatformUseCase.DeleteAsync(idPlatform);
        }

        public List<PlatformDTO> GetAll()
        {
            return _getPlatformUseCase.GetAll();
        }

        public async Task<List<PlatformDTO>> GetAllAsync()
        {
            return await _getPlatformUseCase.GetAllAsync();
        }

        public async Task<PlatformDTO> GetAsync(int idPlatform)
        {
            return await _getPlatformUseCase.GetAsync(idPlatform);
        }

        public async Task<(PlatformDTO? PlatformDTO, string? Message)> UpdateAsync(int idPlatform, string platformName, string platformDescription)
        {
            return await _updatePlatformUseCase.UpdateAsync(idPlatform, platformName, platformDescription);
        }

        public async Task<PlatformDTO> ForcedUpdateAsync(int idPlatform, string platformName, string platformDescription)
        {
            return await _updatePlatformUseCase.ForcedUpdateAsync(idPlatform, platformName, platformDescription);
        }
    }
}

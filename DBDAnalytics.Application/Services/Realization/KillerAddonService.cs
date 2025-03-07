using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.KillerAddonCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class KillerAddonService(ICreateKillerAddonUseCase createKillerAddonUseCase,
                                    IDeleteKillerAddonUseCase deleteKillerAddonUseCase,
                                    IGetKillerAddonUseCase getKillerAddonUseCase,
                                    IUpdateKillerAddonUseCase updateKillerAddonUseCase) : IKillerAddonService
    {
        private readonly ICreateKillerAddonUseCase _createKillerAddonUseCase = createKillerAddonUseCase;
        private readonly IDeleteKillerAddonUseCase _deleteKillerAddonUseCase = deleteKillerAddonUseCase;
        private readonly IGetKillerAddonUseCase _getKillerAddonUseCase = getKillerAddonUseCase;
        private readonly IUpdateKillerAddonUseCase _updateKillerAddonUseCase = updateKillerAddonUseCase;

        public async Task<(KillerAddonDTO? KillerAddonDTO, string Message)> CreateAsync(int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            return await _createKillerAddonUseCase.CreateAsync(idKiller, idRarity, addonName, addonImage, addonDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKillerAddon)
        {
            return await _deleteKillerAddonUseCase.DeleteAsync(idKillerAddon);
        }

        public List<KillerAddonDTO> GetAll()
        {
            return _getKillerAddonUseCase.GetAll();
        }

        public async Task<List<KillerAddonDTO>> GetAllAsync()
        {
            return await _getKillerAddonUseCase.GetAllAsync();
        }

        public async Task<KillerAddonDTO> GetAsync(int idKillerAddon)
        {
            return await _getKillerAddonUseCase.GetAsync(idKillerAddon);
        }

        public async Task<(KillerAddonDTO? KillerAddonDTO, string? Message)> UpdateAsync(int idAddon, int idKiller, int? idRarity, string addonName, byte[]? addonImage, string? addonDescription)
        {
            return await _updateKillerAddonUseCase.UpdateAsync(idAddon, idKiller, idRarity, addonName, addonImage, addonDescription);
        }
    }
}
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class KillerPerkService(ICreateKillerPerkUseCase createKillerPerkUseCase,
                                   IDeleteKillerPerkUseCase deleteKillerPerkUseCase,
                                   IGetKillerPerkUseCase getKillerPerkUseCase,
                                   IUpdateKillerPerkUseCase updateKillerPerkUseCase) : IKillerPerkService
    {
        private readonly ICreateKillerPerkUseCase _createKillerPerkUseCase = createKillerPerkUseCase;
        private readonly IDeleteKillerPerkUseCase _deleteKillerPerkUseCase = deleteKillerPerkUseCase;
        private readonly IGetKillerPerkUseCase _getKillerPerkUseCase = getKillerPerkUseCase;
        private readonly IUpdateKillerPerkUseCase _updateKillerPerkUseCase = updateKillerPerkUseCase;

        public async Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> CreateAsync(int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            return await _createKillerPerkUseCase.CreateAsync(idKiller, perkName, perkImage, idCategory, perkDescription);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idPerk)
        {
            return await _deleteKillerPerkUseCase.DeleteAsync(idPerk);
        }

        public List<KillerPerkDTO> GetAll()
        {
            return _getKillerPerkUseCase.GetAll();
        }

        public async Task<List<KillerPerkDTO>> GetAllAsync()
        {
            return await _getKillerPerkUseCase.GetAllAsync();
        }

        public async Task<KillerPerkDTO> GetAsync(int idPerk)
        {
            return await _getKillerPerkUseCase.GetAsync(idPerk);
        }

        public async Task<(KillerPerkDTO? KillerPerkDTO, string? Message)> UpdateAsync(int idPerk, int idKiller, string perkName, byte[]? perkImage, int? idCategory, string? perkDescription)
        {
            return await _updateKillerPerkUseCase.UpdateAsync(idPerk, idKiller, perkName, perkImage, idCategory, perkDescription);
        }
    }
}
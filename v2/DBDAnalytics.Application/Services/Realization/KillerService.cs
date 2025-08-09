using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.KillerCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class KillerService(ICreateKillerUseCase createKillerUseCase,
                               IDeleteKillerUseCase deleteKillerUseCase,
                               IGetKillerUseCase getKillerUseCase,
                               IGetKillerLoadoutUseCase getKillerLoadoutUseCase,
                               IUpdateKillerUseCase updateKillerUseCase) : IKillerService
    {
        private readonly ICreateKillerUseCase _createKillerUseCase = createKillerUseCase;
        private readonly IDeleteKillerUseCase _deleteKillerUseCase = deleteKillerUseCase;
        private readonly IGetKillerUseCase _getKillerUseCase = getKillerUseCase;
        private readonly IGetKillerLoadoutUseCase _getKillerLoadoutUseCase = getKillerLoadoutUseCase;
        private readonly IUpdateKillerUseCase _updateKillerUseCase = updateKillerUseCase;

        public async Task<(KillerDTO? KillerrDTO, string Message)> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            return await _createKillerUseCase.CreateAsync(killerName, killerImage, killerAbilityImage);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idKiller)
        {
            return await _deleteKillerUseCase.DeleteAsync(idKiller);
        }

        public List<KillerDTO> GetAll()
        {
            return _getKillerUseCase.GetAll();
        }

        public async Task<List<KillerDTO>> GetAllAsync()
        {
            return await _getKillerUseCase.GetAllAsync();
        }

        public async Task<KillerDTO> GetAsync(int idKiller)
        {
            return await _getKillerUseCase.GetAsync(idKiller);
        }

        public async Task<List<KillerLoadoutDTO>> GetKillersWithAddonsAndPerksAsync()
        {
            return await _getKillerLoadoutUseCase.GetKillersWithAddonsAndPerksAsync();
        }

        public async Task<KillerLoadoutDTO> GetKillerWithAddonsAndPerksAsync(int idKiller)
        {
            return await _getKillerLoadoutUseCase.GetKillerWithAddonsAndPerksAsync(idKiller);
        }

        public async Task<(KillerDTO? KillerDTO, string? Message)> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            return await _updateKillerUseCase.UpdateAsync(idKiller, killerName, killerImage, killerAbilityImage);
        }

        public async Task<KillerDTO> ForcedUpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage)
        {
            return await _updateKillerUseCase.ForcedUpdateAsync(idKiller, killerName, killerImage, killerAbilityImage);
        }
    }
}
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.GameModeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;
using System.Diagnostics;

namespace DBDAnalytics.Application.UseCases.Realization.GameModeCase
{
    public class GetGameModeUseCase(IGameModeRepository gameModeRepository) : IGetGameModeUseCase
    {
        private readonly IGameModeRepository _gameModeRepository = gameModeRepository;

        public async Task<List<GameModeDTO>> GetAllAsync()
        {
            var domainEntities = await _gameModeRepository.GetAllAsync();
            
            
            var dtoGameEntities = domainEntities.ToDTO();

            return dtoGameEntities;
        }

        public List<GameModeDTO> GetAll()
        {
            var domainEntities = _gameModeRepository.GetAll();

            var dtoGameEntities = domainEntities.ToDTO();

            return dtoGameEntities;
        }

        public async Task<GameModeDTO?> GetAsync(int idGameMode)
        {
            var domainEntity = await _gameModeRepository.GetAsync(idGameMode);

            if (domainEntity == null)
            {
                Debug.WriteLine($"GameMode с ID {idGameMode} не найден в репозитории.");
                return null;
            }

            var dtoGameEntity = domainEntity.ToDTO();

            return dtoGameEntity;
        }
    }
}

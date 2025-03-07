using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RarityCase
{
    public interface IUpdateRarityUseCase
    {
        Task<(RarityDTO? RarityDTO, string? Message)> UpdateAsync(int idRarity, string rarityName);
    }
}
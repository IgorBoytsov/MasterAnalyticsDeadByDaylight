using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.RarityCase
{
    public interface ICreateRarityUseCase
    {
        Task<(RarityDTO? RarityDTO, string? Message)> CreateAsync(string rarityName, string? description);
    }
}
using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase
{
    public interface ICreateWhoPlacedMapUseCase
    {
        Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> CreateAsync(string whoPlacedMapName);
    }
}
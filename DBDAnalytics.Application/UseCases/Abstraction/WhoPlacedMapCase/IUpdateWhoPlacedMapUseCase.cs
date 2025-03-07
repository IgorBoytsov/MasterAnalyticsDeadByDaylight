using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.WhoPlacedMapCase
{
    public interface IUpdateWhoPlacedMapUseCase
    {
        Task<(WhoPlacedMapDTO? WhoPlacedMapDTO, string? Message)> UpdateAsync(int idWhoPlacedMap, string whoPlacedMapName);
    }
}
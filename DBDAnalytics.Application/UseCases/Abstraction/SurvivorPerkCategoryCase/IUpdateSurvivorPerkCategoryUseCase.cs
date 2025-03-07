using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase
{
    public interface IUpdateSurvivorPerkCategoryUseCase
    {
        Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> UpdateAsync(int idSurvivorPerkCategory, string survivorPerkCategoryName);
    }
}
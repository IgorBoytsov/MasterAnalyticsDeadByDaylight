using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorPerkCategoryCase
{
    public interface ICreateSurvivorPerkCategoryUseCase
    {
        Task<(SurvivorPerkCategoryDTO? SurvivorPerkCategoryDTO, string? Message)> CreateAsync(string survivorPerkCategoryName);
    }
}
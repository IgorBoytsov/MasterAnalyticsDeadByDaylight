using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase
{
    public interface ICreateKillerPerkCategoryUseCase
    {
        Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> CreateAsync(string KillerPerkCategoryName);
    }
}
using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerPerkCategoryCase
{
    public interface IUpdateKillerPerkCategoryUseCase
    {
        Task<(KillerPerkCategoryDTO? KillerPerkCategoryDTO, string? Message)> UpdateAsync(int idKillerPerkCategory, string KillerPerkCategoryName, string? description);
    }
}
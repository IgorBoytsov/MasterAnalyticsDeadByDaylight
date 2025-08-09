using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase
{
    public interface IUpdateOfferingCategoryUseCase
    {
        Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> UpdateAsync(int idOfferingCategory, string offeringCategoryName, string? description);
    }
}
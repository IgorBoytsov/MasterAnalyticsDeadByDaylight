using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCategoryCase
{
    public interface ICreateOfferingCategoryUseCase
    {
        Task<(OfferingCategoryDTO? OfferingCategoryDTO, string? Message)> CreateAsync(string offeringCategoryName);
    }
}
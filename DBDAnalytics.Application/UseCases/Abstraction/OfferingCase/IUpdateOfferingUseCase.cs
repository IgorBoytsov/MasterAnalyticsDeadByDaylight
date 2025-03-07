using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCase
{
    public interface IUpdateOfferingUseCase
    {
        Task<OfferingDTO?> ForcedUpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
        Task<(OfferingDTO? OfferingDTO, string? Message)> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
    }
}
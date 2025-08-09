using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCase
{
    public interface ICreateOfferingUseCase
    {
        Task<(OfferingDTO? OfferingDTO, string Message)> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
    }
}
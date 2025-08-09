using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IOfferingService
    {
        Task<(OfferingDTO? OfferingDTO, string Message)> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idOffering);
        Task<OfferingDTO> ForcedUpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
        List<OfferingDTO> GetAll();
        Task<List<OfferingDTO>> GetAllAsync();
        Task<OfferingDTO> GetAsync(int idOffering);
        Task<(OfferingDTO? OfferingDTO, string? Message)> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription);
    }
}
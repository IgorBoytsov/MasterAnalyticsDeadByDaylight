using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.OfferingCase
{
    public interface IGetOfferingUseCase
    {
        List<OfferingDTO> GetAll();
        Task<List<OfferingDTO>> GetAllAsync();
        Task<OfferingDTO?> GetAsync(int idOffering);
    }
}
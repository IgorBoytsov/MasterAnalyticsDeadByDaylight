using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase
{
    public interface IGetTypeDeathUseCase
    {
        List<TypeDeathDTO> GetAll();
        Task<List<TypeDeathDTO>> GetAllTypeDeathAsync();
        Task<TypeDeathDTO?> GetTypeDeathAsync(int idTypeDeath);
    }
}
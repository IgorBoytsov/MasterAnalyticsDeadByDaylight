using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ITypeDeathService
    {
        Task<(TypeDeathDTO? TypeDeathDTO, string Message)> CreateAsync(string typeDeathName, string typeDeathDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idTypeDeath);
        List<TypeDeathDTO> GetAll();
        Task<List<TypeDeathDTO>> GetAllAsync();
        Task<TypeDeathDTO> GetAsync(int idTypeDeath);
        Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription);
        Task<TypeDeathDTO> ForcedUpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription);
    }
}
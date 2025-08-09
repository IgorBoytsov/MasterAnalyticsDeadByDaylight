using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase
{
    public interface IUpdateTypeDeathUseCase
    {
        Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription);
        Task<TypeDeathDTO?> ForcedUpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription);
    }
}
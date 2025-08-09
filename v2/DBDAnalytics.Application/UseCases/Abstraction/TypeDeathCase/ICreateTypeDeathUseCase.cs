using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase
{
    public interface ICreateTypeDeathUseCase
    {
        Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> CreateAsync(string typeDeathName, string typeDeathDescription);
    }
}
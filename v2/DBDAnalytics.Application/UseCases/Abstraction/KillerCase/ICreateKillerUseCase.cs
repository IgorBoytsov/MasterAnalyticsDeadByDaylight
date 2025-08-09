using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerCase
{
    public interface ICreateKillerUseCase
    {
        Task<(KillerDTO? KillerDTO, string? Message)> CreateAsync(string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
    }
}
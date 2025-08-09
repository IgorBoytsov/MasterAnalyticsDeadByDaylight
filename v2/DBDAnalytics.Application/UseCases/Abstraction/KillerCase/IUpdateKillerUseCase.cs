using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.KillerCase
{
    public interface IUpdateKillerUseCase
    {
        Task<KillerDTO?> ForcedUpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
        Task<(KillerDTO? KillerDTO, string? Message)> UpdateAsync(int idKiller, string killerName, byte[]? killerImage, byte[]? killerAbilityImage);
    }
}
using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase
{
    public interface ICreateMatchAttributeUseCase
    {
        Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide);
    }
}
using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase
{
    public interface IUpdateMatchAttributeUseCase
    {
        Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide);
    }
}
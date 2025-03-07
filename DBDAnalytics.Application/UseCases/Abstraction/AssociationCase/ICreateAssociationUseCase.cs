using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.AssociationCase
{
    public interface ICreateAssociationUseCase
    {
        Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> CreateAsync(string playerAssociationName, string playerAssociationDescription);
    }
}
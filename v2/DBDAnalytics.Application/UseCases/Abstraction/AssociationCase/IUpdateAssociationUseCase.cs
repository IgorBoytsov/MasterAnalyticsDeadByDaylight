using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.AssociationCase
{
    public interface IUpdateAssociationUseCase
    {
        Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription);
        Task<PlayerAssociationDTO?> ForcedUpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription);
    }
}
using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IAssociationService
    {
        Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> CreateAsync(string playerAssociationName, string playerAssociationDescription);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idPlayerAssociation);
        List<PlayerAssociationDTO> GetAll();
        Task<List<PlayerAssociationDTO>> GetAllAsync();
        Task<PlayerAssociationDTO?> GetAsync(int idPlayerAssociation);
        /// <summary>
        /// Метод обновление записи в таблице с проверками.
        /// </summary>
        /// <param name="idPlayerAssociation"></param>
        /// <param name="playerAssociationName"></param>
        /// <param name="playerAssociationDescription"></param>
        /// <returns></returns>
        Task<(PlayerAssociationDTO? PlayerAssociationDTO, string? Message)> UpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription);
        /// <summary>
        /// Метод принудительного обновление записи в таблице.
        /// </summary>
        /// <param name="idPlayerAssociation"></param>
        /// <param name="playerAssociationName"></param>
        /// <param name="playerAssociationDescription"></param>
        /// <returns></returns>
        Task<PlayerAssociationDTO> ForcedUpdateAsync(int idPlayerAssociation, string playerAssociationName, string playerAssociationDescription);
    }
}
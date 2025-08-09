using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IMatchAttributeService
    {
        Task<(MatchAttributeDTO? MatchAttributeDTO, string Message)> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide);
        Task<(bool IsDeleted, string Message)> DeleteAsync(int idMatchAttribute);
        /// <summary>
        /// isHide : true - Не показывать скрытые.
        /// isHide : false - Показывать скрытые.
        /// </summary>
        List<MatchAttributeDTO> GetAll(bool isHide);
        /// <summary>
        /// isHide : true - Не показывать скрытые.
        /// isHide : false - Показывать скрытые.
        /// </summary>
        Task<List<MatchAttributeDTO>> GetAllAsync(bool isHide);
        /// <summary>
        /// isHide : true - Не показывать скрытые.
        /// isHide : false - Показывать скрытые.
        /// </summary>
        Task<MatchAttributeDTO> GetAsync(int idMatchAttribute);
        Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide);
    }
}
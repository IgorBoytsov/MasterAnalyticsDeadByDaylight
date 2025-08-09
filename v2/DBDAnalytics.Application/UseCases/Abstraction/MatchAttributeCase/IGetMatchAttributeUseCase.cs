using DBDAnalytics.Application.DTOs;

namespace DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase
{
    public interface IGetMatchAttributeUseCase
    {
        List<MatchAttributeDTO> GetAll(bool isHide);
        Task<List<MatchAttributeDTO>> GetAllAsync(bool isHide);
        Task<MatchAttributeDTO?> GetAsync(int idMatchAttribute);
    }
}
using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class MatchAttributeService(ICreateMatchAttributeUseCase createMatchAttributeUseCase,
                                       IDeleteMatchAttributeUseCase deleteMatchAttributeUseCase,
                                       IGetMatchAttributeUseCase getMatchAttributeUseCase,
                                       IUpdateMatchAttributeUseCase updateMatchAttributeUseCase) : IMatchAttributeService
    {
        private readonly ICreateMatchAttributeUseCase _createMatchAttributeUseCase = createMatchAttributeUseCase;
        private readonly IDeleteMatchAttributeUseCase _deleteMatchAttributeUseCase = deleteMatchAttributeUseCase;
        private readonly IGetMatchAttributeUseCase _getMatchAttributeUseCase = getMatchAttributeUseCase;
        private readonly IUpdateMatchAttributeUseCase _updateMatchAttributeUseCase = updateMatchAttributeUseCase;

        public async Task<(MatchAttributeDTO? MatchAttributeDTO, string Message)> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide)
        {
            return await _createMatchAttributeUseCase.CreateAsync(attributeName, AttributeDescription, CreatedAt, isHide);
        }

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idMatchAttribute)
        {
            return await _deleteMatchAttributeUseCase.DeleteAsync(idMatchAttribute);
        }

        public List<MatchAttributeDTO> GetAll(bool isHide)
        {
            return _getMatchAttributeUseCase.GetAll(isHide);
        }

        public async Task<List<MatchAttributeDTO>> GetAllAsync(bool isHide)
        {
            return await _getMatchAttributeUseCase.GetAllAsync(isHide);
        }

        public async Task<MatchAttributeDTO> GetAsync(int idMatchAttribute)
        {
            return await _getMatchAttributeUseCase.GetAsync(idMatchAttribute);
        }

        public async Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide)
        {
            return await _updateMatchAttributeUseCase.UpdateAsync(idMatchAttribute, attributeName, AttributeDescription, isHide);
        }
    }
}
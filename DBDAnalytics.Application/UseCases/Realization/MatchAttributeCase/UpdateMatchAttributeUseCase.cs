using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MatchAttributeCase
{
    public class UpdateMatchAttributeUseCase(IMatchAttributeRepository matchAttributeRepository) : IUpdateMatchAttributeUseCase
    {
        private readonly IMatchAttributeRepository _matchAttributeRepository = matchAttributeRepository;

        public async Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> UpdateAsync(int idMatchAttribute, string attributeName, string? AttributeDescription, bool isHide)
        {
            string message = string.Empty;

            if (idMatchAttribute == 0 || string.IsNullOrWhiteSpace(attributeName))
                return (null, "Такой записи не существует");

            var exist = await _matchAttributeRepository.ExistAsync(attributeName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _matchAttributeRepository.UpdateAsync(idMatchAttribute, attributeName, AttributeDescription, isHide);

            var domainEntity = await _matchAttributeRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
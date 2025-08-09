using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.MatchAttributeCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.MatchAttributeCase
{
    public class CreateMatchAttributeUseCase(IMatchAttributeRepository matchAttributeRepository) : ICreateMatchAttributeUseCase
    {
        private readonly IMatchAttributeRepository _matchAttributeRepository = matchAttributeRepository;

        public async Task<(MatchAttributeDTO? MatchAttributeDTO, string? Message)> CreateAsync(string attributeName, string? AttributeDescription, DateTime CreatedAt, bool isHide)
        {
            string message = string.Empty;

            var (CreatedMatchAttribute, Message) = MatchAttributeDomain.Create(0, attributeName, AttributeDescription, CreatedAt, isHide);

            if (CreatedMatchAttribute is null)
            {
                return (null, Message);
            }

            bool exist = await _matchAttributeRepository.ExistAsync(attributeName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _matchAttributeRepository.CreateAsync(attributeName, AttributeDescription, CreatedAt, isHide);

            var domainEntity = await _matchAttributeRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
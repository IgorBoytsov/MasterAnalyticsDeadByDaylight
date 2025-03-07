using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.TypeDeathCase
{
    public class CreateTypeDeathUseCase(ITypeDeathRepository typeDeathRepository) : ICreateTypeDeathUseCase
    {
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> CreateAsync(string typeDeathName, string typeDeathDescription)
        {
            string message = string.Empty;

            var (CreatedTypeDeath, Message) = TypeDeathDomain.Create(0, typeDeathName, typeDeathDescription);

            if (CreatedTypeDeath is null)
            {
                return (null, Message);
            }

            bool exist = await _typeDeathRepository.ExistAsync(typeDeathName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _typeDeathRepository.CreateAsync(typeDeathName, typeDeathDescription);

            var domainEntity = await _typeDeathRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
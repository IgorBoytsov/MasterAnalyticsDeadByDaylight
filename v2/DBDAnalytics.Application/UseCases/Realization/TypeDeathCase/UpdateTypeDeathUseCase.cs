using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.TypeDeathCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.TypeDeathCase
{
    public class UpdateTypeDeathUseCase(ITypeDeathRepository typeDeathRepository) : IUpdateTypeDeathUseCase
    {
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<(TypeDeathDTO? TypeDeathDTO, string? Message)> UpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription)
        {
            string message = string.Empty;

            if (idTypeDeath == 0 || string.IsNullOrWhiteSpace(typeDeathName))
                return (null, "Такой записи не существует");

            var exist = await _typeDeathRepository.ExistAsync(typeDeathName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _typeDeathRepository.UpdateAsync(idTypeDeath, typeDeathName, typeDeathDescription);

            var domainEntity = await _typeDeathRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<TypeDeathDTO?> ForcedUpdateAsync(int idTypeDeath, string typeDeathName, string typeDeathDescription)
        {
            int id = await _typeDeathRepository.UpdateAsync(idTypeDeath, typeDeathName, typeDeathDescription);

            var domainEntity = await _typeDeathRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}
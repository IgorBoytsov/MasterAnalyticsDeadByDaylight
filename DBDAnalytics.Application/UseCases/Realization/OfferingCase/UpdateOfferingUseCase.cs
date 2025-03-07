using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCase
{
    public class UpdateOfferingUseCase(IOfferingRepository offeringRepository) : IUpdateOfferingUseCase
    {
        private readonly IOfferingRepository _offeringRepository = offeringRepository;

        public async Task<(OfferingDTO? OfferingDTO, string? Message)> UpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            string message = string.Empty;

            if (idOffering == 0 || string.IsNullOrWhiteSpace(offeringName))
                return (null, "Такой записи не существует");

            var exist = await _offeringRepository.ExistAsync(offeringName);

            if (exist)
                return (null, "Название на которое вы хотите поменять - уже существует.");

            int id = await _offeringRepository.UpdateAsync(idOffering, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);

            var domainEntity = await _offeringRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return (null, "Не удалось получить обновляемую запись");
            }

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }

        public async Task<OfferingDTO?> ForcedUpdateAsync(int idOffering, int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            int id = await _offeringRepository.UpdateAsync(idOffering, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);

            var domainEntity = await _offeringRepository.GetAsync(id);

            if (domainEntity == null)
            {
                return null;
            }

            var dtoEntity = domainEntity.ToDTO();

            return dtoEntity;
        }
    }
}

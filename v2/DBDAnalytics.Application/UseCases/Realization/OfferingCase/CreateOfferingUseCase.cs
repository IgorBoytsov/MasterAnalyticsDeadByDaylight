using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Application.Mappers;
using DBDAnalytics.Application.UseCases.Abstraction.OfferingCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.OfferingCase
{
    public class CreateOfferingUseCase(IOfferingRepository offeringRepository) : ICreateOfferingUseCase
    {
        private readonly IOfferingRepository _offeringRepository = offeringRepository;

        public async Task<(OfferingDTO? OfferingDTO, string Message)> CreateAsync(int idRole, int idCategory, int idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            string message = string.Empty;

            var (CreatedOffering, Message) = OfferingDomain.Create(0, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);

            if (CreatedOffering is null)
            {
                return (null, Message);
            }

            bool exist = await _offeringRepository.ExistAsync(offeringName);

            if (exist)
                return (null, "Запись с таким названием уже существует.");

            var id = await _offeringRepository.CreateAsync(idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);

            var domainEntity = await _offeringRepository.GetAsync(id);

            var dtoEntity = domainEntity.ToDTO();

            return (dtoEntity, message);
        }
    }
}
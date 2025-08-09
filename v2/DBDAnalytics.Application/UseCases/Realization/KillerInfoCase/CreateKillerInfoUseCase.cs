using DBDAnalytics.Application.UseCases.Abstraction.KillerInfoCase;
using DBDAnalytics.Domain.DomainModels;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.KillerInfoCase
{
    public class CreateKillerInfoUseCase(IKillerInfoRepository killerInfoRepository) : ICreateKillerInfoUseCase
    {
        private readonly IKillerInfoRepository _killerRepository = killerInfoRepository;

        public (int IdKillerInfo, string? Message) Create(
                          int idKiller,
                          int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4,
                          int idAddon1, int idAddon2,
                          int idAssociation, int idPlatform, int? idKillerOffering,
                          int prestige, bool isBot, bool isAnonymousMode, int killerAccount)
        {
            string message = string.Empty;

            //var (CreatedKillerInfo, Message) = KillerInfoDomain.Create(0, idKiller, idPerk1, idPerk2, idPerk3, idPerk4, idAddon1, idAddon2, idAssociation, idPlatform, idKillerOffering, prestige, isBot, isAnonymousMode, killerAccount);

            //if (CreatedKillerInfo is null)
            //{
            //    return (0, Message);
            //}

            var id = _killerRepository.Create(
                     idKiller: idKiller,
                     idPerk1: idPerk1, idPerk2: idPerk2, idPerk3: idPerk3, idPerk4: idPerk4,
                     idAddon1: idAddon1, idAddon2: idAddon2,
                     idAssociation: idAssociation, idPlatform: idPlatform, idKillerOffering: idKillerOffering,
                     prestige: prestige, isBot: isBot, isAnonymousMode: isAnonymousMode, killerAccount: killerAccount);

            return (id, message);
        }
    }
}
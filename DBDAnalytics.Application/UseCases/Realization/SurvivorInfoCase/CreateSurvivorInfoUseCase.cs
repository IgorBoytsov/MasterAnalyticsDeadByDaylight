using DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.SurvivorInfoCase
{
    public class CreateSurvivorInfoUseCase(ISurvivorInfoRepository survivorInfoRepository) : ICreateSurvivorInfoUseCase
    {
        private readonly ISurvivorInfoRepository _survivorInfoRepository = survivorInfoRepository;

        public (int IdSurvivorInfo, string? Message) Create(
                          int idSurvivor,
                          int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk,
                          int idItem, int idFirstItemAddon, int idSecondItemAddon,
                          int idTypeDeath, int idAssociation, int idPlatform, int IdOffering,
                          int prestige, bool isBot, bool isAnonymousMode, int survivorAccount)
        {
            string message = string.Empty;

            if (prestige > 100)
                return (0, "Престиж не может быть больше 100.");

            var id = _survivorInfoRepository.Create(
                     idSurvivor: idSurvivor,
                     idFirstPerk: idFirstPerk, idSecondPerk: idSecondPerk, idThirdPerk: idThirdPerk, idFourthPerk: idFourthPerk,
                     idItem: idItem, idFirstItemAddon: idFirstItemAddon, idSecondItemAddon: idSecondItemAddon,
                     idTypeDeath: idTypeDeath, idAssociation: idAssociation, idPlatform: idPlatform, IdOffering: IdOffering,
                     prestige: prestige, isBot: isBot, isAnonymousMode: isAnonymousMode, survivorAccount: survivorAccount);

            return (id, message);
        }
    }
}
using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class SurvivorInfoService(ICreateSurvivorInfoUseCase createSurvivorInfoUseCase,
                                     IGetSurvivorInfoUseCase getSurvivorInfoUseCase) : ISurvivorInfoService
    {
        private readonly ICreateSurvivorInfoUseCase _createSurvivorInfoUseCase = createSurvivorInfoUseCase;
        private readonly IGetSurvivorInfoUseCase _getSurvivorInfoUseCase = getSurvivorInfoUseCase;

        public (int IdSurvivorInfo, string? Message) Create(
                int idSurvivor,
                int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk,
                int idItem, int idFirstItemAddon, int idSecondItemAddon,
                int idTypeDeath, int idAssociation, int idPlatform, int IdOffering,
                int prestige, bool isBot, bool isAnonymousMode, int survivorAccount)
        {
            return _createSurvivorInfoUseCase.Create(
                     idSurvivor: idSurvivor,
                     idFirstPerk: idFirstPerk, idSecondPerk: idSecondPerk, idThirdPerk: idThirdPerk, idFourthPerk: idFourthPerk,
                     idItem: idItem, idFirstItemAddon: idFirstItemAddon, idSecondItemAddon: idSecondItemAddon,
                     idTypeDeath: idTypeDeath, idAssociation: idAssociation, idPlatform: idPlatform, IdOffering: IdOffering,
                     prestige: prestige, isBot: isBot, isAnonymousMode: isAnonymousMode, survivorAccount: survivorAccount);
        }

        public (List<int>? IdRecords, string? Message) GetLastNRecordsId(int countRecords)
        {
            return _getSurvivorInfoUseCase.GetLastNRecordsId(countRecords);
        }
    }
}
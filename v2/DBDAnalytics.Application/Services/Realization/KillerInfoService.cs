using DBDAnalytics.Application.Services.Abstraction;
using DBDAnalytics.Application.UseCases.Abstraction.KillerInfoCase;

namespace DBDAnalytics.Application.Services.Realization
{
    public class KillerInfoService(ICreateKillerInfoUseCase createKillerInfoUseCase) : IKillerInfoService
    {
        private readonly ICreateKillerInfoUseCase _createKillerInfoUseCase = createKillerInfoUseCase;

        public (int IdKillerInfo, string? Message) Create(
                int idKiller,
                int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4,
                int idAddon1, int idAddon2,
                int idAssociation, int idPlatform, int? idKillerOffering,
                int prestige, bool isBot, bool isAnonymousMode, int killerAccount)
        {
            return _createKillerInfoUseCase.Create(
                     idKiller: idKiller,
                     idPerk1: idPerk1, idPerk2: idPerk2, idPerk3: idPerk3, idPerk4: idPerk4,
                     idAddon1: idAddon1, idAddon2: idAddon2,
                     idAssociation: idAssociation, idPlatform: idPlatform, idKillerOffering: idKillerOffering,
                     prestige: prestige, isBot: isBot, isAnonymousMode: isAnonymousMode, killerAccount: killerAccount);
        }
    }
}
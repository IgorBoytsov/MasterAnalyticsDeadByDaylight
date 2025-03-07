namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface IKillerInfoService
    {
        (int IdKillerInfo, string? Message) Create(int idKiller, int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4, int idAddon1, int idAddon2, int idAssociation, int idPlatform, int? idKillerOffering, int prestige, bool isBot, bool isAnonymousMode, int killerAccount);
    }
}
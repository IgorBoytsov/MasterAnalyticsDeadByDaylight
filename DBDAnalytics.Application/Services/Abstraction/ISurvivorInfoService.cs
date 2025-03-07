namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ISurvivorInfoService
    {
        (int IdSurvivorInfo, string? Message) Create(int idSurvivor, int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk, int idItem, int idFirstItemAddon, int idSecondItemAddon, int idTypeDeath, int idAssociation, int idPlatform, int IdOffering, int prestige, bool isBot, bool isAnonymousMode, int survivorAccount);
        (List<int>? IdRecords, string? Message) GetLastNRecordsId(int countRecords);
    }
}
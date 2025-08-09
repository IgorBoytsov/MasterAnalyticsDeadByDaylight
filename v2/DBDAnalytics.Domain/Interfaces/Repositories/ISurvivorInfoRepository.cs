namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface ISurvivorInfoRepository
    {
        int Create(int idSurvivor, int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk, int idItem, int idFirstItemAddon, int idSecondItemAddon, int idTypeDeath, int idAssociation, int idPlatform, int IdOffering, int prestige, bool isBot, bool isAnonymousMode, int survivorAccount);
        List<int> GetLastNRecordsId(int countRecords);
    }
}
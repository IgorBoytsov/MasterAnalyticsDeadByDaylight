namespace DBDAnalytics.Application.UseCases.Abstraction.SurvivorInfoCase
{
    public interface ICreateSurvivorInfoUseCase
    {
        (int IdSurvivorInfo, string? Message) Create(int idSurvivor, int idFirstPerk, int idSecondPerk, int idThirdPerk, int idFourthPerk, int idItem, int idFirstItemAddon, int idSecondItemAddon, int idTypeDeath, int idAssociation, int idPlatform, int IdOffering, int prestige, bool isBot, bool isAnonymousMode, int survivorAccount);
    }
}
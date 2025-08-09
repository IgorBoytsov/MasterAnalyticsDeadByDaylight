namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorInfoDomain
    {
        private SurvivorInfoDomain(
            int idSurvivorInfo, int idSurvivor, int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4,int idItem, int idItemAddon1, int idItemAddon2, int idTypeDeath, int idAssociation, int idPlatform, int? idSurvivorOffering,
            int prestige, bool bot, bool anonymousMode, int survivorAccount)
        {
            IdSurvivorInfo = idSurvivorInfo;
            IdSurvivor = idSurvivor;
            IdPerk1 = idPerk1;
            IdPerk2 = idPerk2;
            IdPerk3 = idPerk3;
            IdPerk4 = idPerk4;
            IdItem = idItem;
            IdAddon1 = idItemAddon1;
            IdAddon2 = idItemAddon2;
            IdTypeDeath = idTypeDeath;
            IdAssociation = idAssociation;
            IdPlatform = idPlatform;
            IdSurvivorOffering = idSurvivorOffering;
            Prestige = prestige;
            Bot = bot;
            AnonymousMode = anonymousMode;
            SurvivorAccount = survivorAccount;
        }

        public int IdSurvivorInfo { get; private set; }

        public int IdSurvivor { get; private set; }

        public int? IdPerk1 { get; private set; }

        public int? IdPerk2 { get; private set; }

        public int? IdPerk3 { get; private set; }

        public int? IdPerk4 { get; private set; }

        public int? IdItem { get; private set; }

        public int? IdAddon1 { get; private set; }

        public int? IdAddon2 { get; private set; }

        public int IdTypeDeath { get; private set; }

        public int IdAssociation { get; private set; }

        public int IdPlatform { get; private set; }

        public int? IdSurvivorOffering { get; private set; }

        public int Prestige { get; private set; }

        public bool Bot { get; private set; }

        public bool AnonymousMode { get; private set; }

        public int SurvivorAccount { get; private set; }

        public static (SurvivorInfoDomain? SurvivorInfoDomain, string? Message) Create(
            int idSurvivorInfo, int idSurvivor, int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4, int idItem, int idItemAddon1, int idItemAddon2, int idTypeDeath, int idAssociation, int idPlatform, int? idSurvivorOffering,
            int prestige, bool bot, bool anonymousMode, int survivorAccount)
        {
            string message = string.Empty;
            const int MaxPrestige = 100;
            const int MimPrestige = 0;

            if (idSurvivor < 0)
            {
                return (null, "Выберите выжившего.");
            }

            if (idPerk1 < 0 && idPerk2 < 0 && idPerk3 < 0 && idPerk4 < 0)
            {
                return (null, "Вы не выбрали перк.");
            }

            if (idTypeDeath < 0)
            {
                return (null, "Вы не выбрали тип смерти у выжившего.");
            }

            if (idAssociation < 0)
            {
                return (null, "Вы не выбрали игровую ассоциацию для выжившего.");
            }

            if (idPlatform < 0)
            {
                return (null, "Вы не выбрали игровую платформу для выжившего.");
            }

            if (idSurvivorOffering < 0)
            {
                return (null, "Вы не выбрали подношение для выжившего.");
            }
            
            if (idItem < 0 && idItemAddon1 < 0 && idItemAddon2 < 0)
            {
                return (null, "Вы не выбрали предмет и\\или улучшения к нему для выживших.");
            }

            if (prestige < MimPrestige || prestige > MaxPrestige)
            {
                return (null, $"Престиж не может быть меньше {MimPrestige} и больше {MaxPrestige}.");
            }

            var info = new SurvivorInfoDomain(idSurvivorInfo, idSurvivor, idPerk1, idPerk2, idPerk3, idPerk4, idItem, idItemAddon1, idItemAddon2, idTypeDeath, idAssociation, idPlatform, idSurvivorOffering, prestige, bot, anonymousMode, survivorAccount);

            return (info, message);
        }
    }
}
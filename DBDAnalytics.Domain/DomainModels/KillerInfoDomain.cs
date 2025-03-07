namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerInfoDomain
    {
        private KillerInfoDomain(int idKillerInfo, int idKiller, int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4, int? idAddon1, int? idAddon2, int idAssociation, int idPlatform, int? idKillerOffering,
                                 int prestige, bool bot, bool anonymousMode, int killerAccount)
        {
            IdKillerInfo = idKillerInfo;
            IdKiller = idKiller;
            IdPerk1 = idPerk1;
            IdPerk2 = idPerk2;
            IdPerk3 = idPerk3;
            IdPerk4 = idPerk4;
            IdAddon1 = idAddon1;
            IdAddon2 = idAddon2;
            IdAssociation = idAssociation;
            IdPlatform = idPlatform;
            IdKillerOffering = idKillerOffering;
            Prestige = prestige;
            Bot = bot;
            AnonymousMode = anonymousMode;
            KillerAccount = killerAccount;
        }
        public int IdKillerInfo { get; private set; }

        public int IdKiller { get; private set; }

        public int? IdPerk1 { get; private set; }

        public int? IdPerk2 { get; private set; }

        public int? IdPerk3 { get; private set; }

        public int? IdPerk4 { get; private set; }

        public int? IdAddon1 { get; private set; }

        public int? IdAddon2 { get; private set; }

        public int IdAssociation { get; private set; }

        public int IdPlatform { get; private set; }

        public int? IdKillerOffering { get; private set; }

        public int Prestige { get; private set; }

        public bool Bot { get; private set; }

        public bool AnonymousMode { get; private set; }

        public int KillerAccount { get; private set; }

        public static (KillerInfoDomain? KillerInfoDomain, string? Message) Create(
            int idKillerInfo, int idKiller, int? idPerk1, int? idPerk2, int? idPerk3, int? idPerk4, int? idAddon1, int? idAddon2, int idAssociation, int idPlatform, int? idKillerOffering,
            int prestige, bool bot, bool anonymousMode, int killerAccount)
        {
            string message = string.Empty;
            const int MaxPrestige = 100;
            const int MimPrestige = 0;

            //if (idKiller < 0)
            //{
            //    return (null, "Выберите киллера.");
            //}

            //if (idPerk1 < 0 && idPerk2 < 0 && idPerk3 < 0 && idPerk4 < 0)
            //{
            //    return (null, "Вы не выбрали перк.");
            //}

            //if (idAddon1 < 0 && idAddon2 < 0)
            //{
            //    return (null, "Вы не выбрали улучшение.");
            //}
            
            //if (idAssociation < 0)
            //{
            //    return (null, "Вы не выбрали игровую ассоциацию для киллера.");
            //} 

            //if (idPlatform < 0)
            //{
            //    return (null, "Вы не выбрали игровую платформу для киллера.");
            //}

            //if (idKillerOffering < 0)
            //{
            //    return (null, "Вы не выбрали подношение у киллера.");
            //}

            //if (prestige < MimPrestige || prestige > MaxPrestige)
            //{
            //    return (null, $"Престиж не может быть меньше {MimPrestige} и больше {MaxPrestige}.");
            //}
               
            var killerInfo = new KillerInfoDomain(idKillerInfo, idKiller, idPerk1, idPerk2, idPerk3, idPerk4, idAddon1, idAddon2, idAssociation, idPlatform, idKillerOffering, prestige, bot, anonymousMode, killerAccount);

            return (killerInfo, message);
        }
    }
}
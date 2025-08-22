namespace DBDAnalytics.Shared.Domain.Constants
{
    public static class FileStoragePaths
    {
        // Базовые категории
        public const string KillersBase = "Killers";

        //Общие
        public const string Perks = "Perks";
        public const string Addons = "Addons";
        public const string Maps = "Maps";
        public const string Offerings = "Offerings";

        public const string KillerRole = "KillerRole";
        public const string SurvivorRole = "SurvivorRole";
        public const string GeneralRole = "GeneralRole";

        /// <summary>
        /// "Killers/Portraits"
        /// </summary>
        public const string KillerPortraits = KillersBase + "/Portraits";

        /// <summary>
        /// "Killers/Abilities"
        /// </summary>
        public const string KillerAbilities = KillersBase + "/Abilities";


        /// <summary>
        /// "Offerings/KillerRole"
        /// </summary>
        public static string OfferingKiller => $"{Offerings}/{KillerRole}";

        /// <summary>
        /// "Offerings/SurvivorRole"
        /// </summary>
        public static string OfferingSurvivor => $"{Offerings}/{SurvivorRole}";

        /// <summary>
        /// "Offerings/GeneralRole"
        /// </summary>
        public static string OfferingGeneral => $"{Offerings}/{GeneralRole}";


        /// <summary>
        /// Killers/Perks/Trapper
        /// </summary>
        public static string KillerPerks(string killerName) => $"{KillersBase}/{Perks}/{Sanitize(killerName)}";

        /// <summary>
        /// Killers/Addons/Trapper
        /// </summary>
        public static string KillerAddons(string killerName) => $"{KillersBase}/{Addons}/{Sanitize(killerName)}";

        //private static string Sanitize(string input) => input.Trim().ToLower().Replace(" ", "-");

        /// <summary>
        /// "  the Trapper  " -> "The-trapper"
        /// </summary>
        private static string Sanitize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string processedInput = input.Trim().ToLowerInvariant();
            string capitalized = char.ToUpperInvariant(processedInput[0]) + processedInput[1..];
            return capitalized.Replace(" ", "-");
        }
    }
}
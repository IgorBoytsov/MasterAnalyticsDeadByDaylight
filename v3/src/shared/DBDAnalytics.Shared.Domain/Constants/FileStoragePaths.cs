namespace DBDAnalytics.Shared.Domain.Constants
{
    public static class FileStoragePaths
    {
        // Базовые категории
        public const string KillersBase = "Killers";

        //Общие
        public const string Perks = "Perks";
        public const string Addons = "Addons";

        /// <summary>
        /// "Killers/Portraits"
        /// </summary>
        public const string KillerPortraits = KillersBase + "/Portraits";

        /// <summary>
        /// "Killers/Abilities"
        /// </summary>
        public const string KillerAbilities = KillersBase + "/Abilities";

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
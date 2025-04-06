namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class LoadoutPopularity
    {
        public byte[]? Image { get; set; }

        public string? Name { get; set; }

        public int CountMatchWithItemLoadout { get; set; }

        public int CountWinMatchWithItemLoadout { get; set; }

        public double PickRate { get; set; }

        public double WinRateAllMatch { get; set; }

        public double WinRateWithItemLoadoutMatch { get; set; }
    }
}
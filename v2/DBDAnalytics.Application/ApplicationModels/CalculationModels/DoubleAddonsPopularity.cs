namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class DoubleAddonsPopularity<TAddon> where TAddon : class
    {
        public TAddon FirstAddon { get; set; } = null!;

        public TAddon SecondAddon { get; set; } = null!;

        public int Count { get; set; }

        public double PickRate { get; set; }

        public double WinRate { get; set; }
    }
}
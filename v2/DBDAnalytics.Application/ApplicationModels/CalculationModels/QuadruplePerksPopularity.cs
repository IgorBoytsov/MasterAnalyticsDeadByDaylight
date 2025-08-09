namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class QuadruplePerksPopularity<TPerk> where TPerk : class
    {
        public TPerk FirstPerk { get; set; } = null!;

        public TPerk SecondPerk { get; set; } = null!;

        public TPerk ThirdPerk { get; set; } = null!;

        public TPerk FourthPerk { get; set; } = null!;

        public int Count { get; set; }

        public double PickRate { get; set; }

        public double WinRate { get; set; }
    }
}
namespace DBDAnalytics.Application.ApplicationModels.CalculationModels
{
    public class TextStatistic
    {
        public string? TotalTimeMatches { get; set; }

        public string? AverageTimeMatch { get; set; }

        public string? LongestTimeMatch { get; set; }

        public string? ShortestTimeMatch { get; set; }

        //Победы
        public string? LongestTimeWinMatch { get; set; }

        public string? ShortestTimeWinMatch { get; set; }

        public string? AverageTimeWinMatch { get; set; }

        public int SeriesWins { get; set; }


        public int SeriesWinsWithoutPerks { get; set; }

        public int SeriesWinsWithOnePerk { get; set; }

        public int SeriesWinsWithTwoPerks { get; set; }

        public int SeriesWinsWithThreePerks { get; set; }

        public int SeriesWinsWithFourPerks { get; set; }

        //Ничья
        public string? LongestTimeDrawMatch { get; set; }

        public string? ShortestTimeDrawMatch { get; set; }

        public string? AverageTimeDrawMatch { get; set; }

        public int SeriesDraws { get; set; }


        public int SeriesDrawWithoutPerks { get; set; }

        public int SeriesDrawWithOnePerk { get; set; }

        public int SeriesDrawWithTwoPerks { get; set; }

        public int SeriesDrawWithThreePerks { get; set; }

        public int SeriesDrawWithFourPerks { get; set; }

        //Поражение
        public string? LongestTimeDefeatMatch { get; set; }

        public string? ShortestTimeDefeatMatch { get; set; }

        public string? AverageTimeDefeatMatch { get; set; }

        public int SeriesDefeats { get; set; }


        public int SeriesDefeatsWithoutPerks { get; set; }

        public int SeriesDefeatsWithOnePerk { get; set; }

        public int SeriesDefeatsWithTwoPerks { get; set; }

        public int SeriesDefeatsWithThreePerks { get; set; }

        public int SeriesDefeatsWithFourPerks { get; set; }
    }
}
using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationMap
    {
        public static async Task<double> PickRateAsync(int selectedMatchesOnMap, int countAllMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)selectedMatchesOnMap / countAllMatch * 100, 2);
            });
        }

        public static async Task<int> EscapeSurvivorAsync(int countMatch, int countKill)
        {
            return await Task.Run(() =>
            {
                return countMatch * 4 - (int)countKill;
            });
        }

        public static async Task<double> EscapeRateAsync(int escapeSurvivor, int countMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)escapeSurvivor / (countMatch * 4) * 100, 2);
            });
        }

        public static async Task<double> WinRateRateAsync(int SelectedMatchesWon, int SelectedAllMatch)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)SelectedMatchesWon / SelectedAllMatch * 100, 2);
            });
        }

        public static async Task<int> RandomMapAsync(List<GameStatistic> matches)
        {
            return await Task.Run(() =>
            {
                return matches.Where(x => x.IdWhoPlacedMapWin == 1).Count();
            });
        }

        public static async Task<int> WithOfferingsAsync(List<GameStatistic> matches)
        {
            return await Task.Run(() =>
            {
                return matches.Where(x => x.IdWhoPlacedMapWin == 2 | x.IdWhoPlacedMapWin == 3 | x.IdWhoPlacedMapWin == 4).Count();
            });
        }

        public static async Task<double> DropMapPercentAsync(int countMatchOnMap, int CountAllMatches)
        {
            return await Task.Run(() =>
            {
                return Math.Round((double)countMatchOnMap / CountAllMatches * 100, 2);
            });
        }
    }
}
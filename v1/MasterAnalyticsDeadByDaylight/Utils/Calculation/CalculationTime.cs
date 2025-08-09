using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Utils.Calculation
{
    public static class CalculationTime
    {
        public static Task<(DateTime First, DateTime Last)> FirstAndLastDateMatchAsync(List<GameStatistic> Matches)
        {
            return Task.Run(() =>
            {
                var First = Matches.OrderBy(x => x.DateTimeMatch).FirstOrDefault();
                var Last = Matches.OrderBy(x => x.DateTimeMatch).LastOrDefault();

                DateTime FirstMatch = First.DateTimeMatch.Value;
                DateTime LastMatch = Last.DateTimeMatch.Value;

                return (FirstMatch, LastMatch);
            });
        }

        public static async Task<List<GameStatistic>> MatchForPeriodTimeAsync(List<GameStatistic> Matches, TypeTime typeTime, DateTime periodTime)
        {
            return await Task.Run(() =>
            {
                return Matches.Where(x =>
                       x.DateTimeMatch.HasValue &&
                       (typeTime == TypeTime.Year ? x.DateTimeMatch.Value.Year == periodTime.Year :
                       typeTime == TypeTime.Month ? x.DateTimeMatch.Value.Year == periodTime.Year && x.DateTimeMatch.Value.Month == periodTime.Month :
                       x.DateTimeMatch.Value.Date == periodTime.Date))
                       .ToList();
            });
        }

        public static async Task<List<DateTime>> GetHourListAsync()
        {
            return await Task.Run(() =>
            {
                var hoursList = new List<DateTime>();
                DateTime startDate = DateTime.MinValue;

                for (int i = 0; i < 24; i++)
                {
                    hoursList.Add(startDate.AddHours(i));
                }
                return hoursList;
            });
        }

        public static async Task<(string Longest, string Fastest, string AVG)> StatTimeMatchAsync(List<GameStatistic> Matches, int idKiller, int IdPlayerAssociation)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (Matches.Count == 0)
                        return ("0", "0", "0");

                    TimeSpan[] timeSpans = Matches
                                   .Where(x => x.IdKillerNavigation.IdKiller == idKiller && x.IdKillerNavigation.IdAssociation == IdPlayerAssociation)
                                        .Select(s => TimeSpan.Parse(s.GameTimeMatch)).ToArray();


                    TimeSpan totalTime = TimeSpan.Zero;
                    foreach (var timeSpan in timeSpans)
                    {
                        totalTime += timeSpan;
                    }

                    TimeSpan averageMatchTime = TimeSpan.FromTicks(totalTime.Ticks / timeSpans.Length);
                    averageMatchTime = new TimeSpan(averageMatchTime.Hours, averageMatchTime.Minutes, averageMatchTime.Seconds);

                    return (timeSpans.Max().ToString(), timeSpans.Min().ToString(), averageMatchTime.ToString());
                }
                catch (Exception)
                {
                    return ("0", "0", "0");
                }
                
            });
        }

        public static async Task<TimeSpan> TotalTime(List<GameStatistic> Matches)
        {
            return await Task.Run(() => 
            {
                if (Matches.Count == 0)
                    return TimeSpan.Zero;

                TimeSpan[] timeSpans = Matches.Select(s => TimeSpan.Parse(s.GameTimeMatch)).ToArray();

                TimeSpan totalTime = TimeSpan.Zero;

                foreach (var timeSpan in timeSpans)
                {
                    totalTime += timeSpan;
                }

                return totalTime;

            });
        }
    }
}
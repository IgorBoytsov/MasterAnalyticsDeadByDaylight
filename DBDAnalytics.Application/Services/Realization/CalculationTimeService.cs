using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Application.Services.Abstraction;
using System.Diagnostics;
using System.Globalization;

namespace DBDAnalytics.Application.Services.Realization
{
    public class CalculationTimeService : ICalculationTimeService
    {
        public (TimeSpan TotalTime, TimeSpan LongestTime, TimeSpan ShortestTime, TimeSpan AverageTime) CalculateTimeStats(List<DetailsMatchDTO> Matches)
        {
            try
            {
                if (Matches.Count == 0)
                    return (TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

                TimeSpan[] timeSpans = Matches.Select(s => TimeSpan.Parse(s.DurationMatch)).ToArray();

                if (timeSpans.Length == 0)
                    return (TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);

                TimeSpan totalTime = TimeSpan.Zero;
                foreach (var timeSpan in timeSpans)
                    totalTime += timeSpan;

                TimeSpan averageMatchTime = TimeSpan.FromTicks(totalTime.Ticks / timeSpans.Length);
                averageMatchTime = new TimeSpan(averageMatchTime.Hours, averageMatchTime.Minutes, averageMatchTime.Seconds);

                return (totalTime, timeSpans.Max(), timeSpans.Min(), averageMatchTime);
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"Ошибка парсинга в TimeSpan: {ex.Message}");
                return (TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
                throw;
            }
        }

        public int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public DateTime GetIso8601WeekStart(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);

            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);

            int diff = time.DayOfWeek - DayOfWeek.Monday;

            if (diff < 0)
                diff += 7;

            return time.AddDays(-1 * diff).Date;
        }

        public int GetWeekOfMonth(DateTime date)
        {
            DateTime firstDayOfMonth = new(date.Year, date.Month, 1);
            int weekNumber = (date.Day + (int)firstDayOfMonth.DayOfWeek + 6) / 7; // +6 часов /7 минут на неделю, начиная с понедельника
            return weekNumber;
        }

        public List<DayOfWeek> GetDayOfWeeks() => Enum.GetValues<DayOfWeek>().Skip(1).Concat(Enum.GetValues<DayOfWeek>().Take(1)).ToList();

        public string FormatTimeSpanAdaptive(TimeSpan timeSpan) =>
            timeSpan.TotalHours > 24 ? $"{timeSpan.Days}д {timeSpan.Hours}ч {timeSpan.Minutes}м" : $"{timeSpan.Hours}ч {timeSpan.Minutes}м {timeSpan.Seconds}с";
    }
}

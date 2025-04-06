using DBDAnalytics.Application.DTOs.DetailsDTOs;

namespace DBDAnalytics.Application.Services.Abstraction
{
    public interface ICalculationTimeService
    {
        public (TimeSpan TotalTime, TimeSpan LongestTime, TimeSpan ShortestTime, TimeSpan AverageTime) CalculateTimeStats(List<DetailsMatchDTO> Matches);
        int GetIso8601WeekOfYear(DateTime time);
        DateTime GetIso8601WeekStart(DateTime time);
        int GetWeekOfMonth(DateTime date);
        public List<DayOfWeek> GetDayOfWeeks();
        /// <summary>
        /// Вот этот комментарий должен отображаться.
        /// > 24ч = Дни:Часы:Минуты
        /// < 24ч = Часы:Минуты:Секунды
        /// </summary>
        /// <param name="timeSpan">Время для форматирования.</param>
        /// <returns>Форматированная строка.</returns>
        public string FormatTimeSpanAdaptive(TimeSpan timeSpan);
    }
}
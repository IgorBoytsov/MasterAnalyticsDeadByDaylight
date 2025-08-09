namespace DBDAnalytics.Domain.DomainModels
{
    public class GameStatisticSurvivorFilterDomain
    {
        #region Выживший

        public int? IdSurvivor { get; set; }

        public int? IdOpponentKiller { get; set; }

        #endregion

        #region Игра

        /*--Игровой режим---------------------------------------------------------------------------------*/

        public int? IdGameMode { get; set; }

        public int? IdGameEvent { get; set; }

        /*--Дата и время----------------------------------------------------------------------------------*/

        public bool IsConsiderDateTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        /*--Атрибуты матча--------------------------------------------------------------------------------*/

        public int? IdPatch { get; set; }

        public int? IdMatchAttribute { get; set; }

        #endregion
    }
}
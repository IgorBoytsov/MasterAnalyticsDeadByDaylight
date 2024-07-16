namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    class KillerStat
    {
        /// <summary>
        /// ID киллера
        /// </summary>
        public int KillerID { get; set; }

        /// <summary>
        /// Имя киллера
        /// </summary>
        public string KillerName { get; set; }

        /// <summary>
        /// Портрет киллера
        /// </summary>
        public byte[] KillerImage { get; set;}

        /// <summary>
        /// Количество сыгранных игр за данного персонажа в %
        /// </summary>
        public double KillerCountGame { get; set; }

        /// <summary>
        /// Пикрейт киллера, как часто игрок играет за данного персонажа в %
        /// </summary>
        public double KillerPickRate { get; set; }

        /// <summary>
        /// Килрейт киллера, Среднее количество киллов
        /// </summary>
        public double KillerKillRate { get; set; }

        /// <summary>
        /// Килрейт киллера в %
        /// </summary>
        public double KillerKillRatePercentage { get; set; }

        /// <summary>
        /// Винрейт киллера в %
        /// </summary>
        public double KillerWinRate { get; set; }

        /// <summary>
        /// Количество выигранных матчей
        /// </summary>
        public double KillerMatchWin{ get; set; }

        /// <summary>
        /// Количество игр, в которых игрок на киллере сделал минус 0 %
        /// </summary>
        public double KillingZeroSurvivor { get; set; }

        /// <summary>
        /// Количество игр, в которых игрок на киллере сделал минус 1 %
        /// </summary>
        public double KillingOneSurvivors { get; set; }

        /// <summary>
        /// Количество игр, в которых игрок на киллере сделал минус 2 %
        /// </summary>
        public double KillingTwoSurvivors { get; set; }

        /// <summary>
        /// Количество игр, в которых игрок на киллере сделал минус 3 %
        /// </summary>
        public double KillingThreeSurvivors { get; set; }

        /// <summary>
        /// Количество игр, в которых игрок на киллере сделал минус 4 %
        /// </summary>
        public double KillingFourSurvivors { get; set; }
    }
}

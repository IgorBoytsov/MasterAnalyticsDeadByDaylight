﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    class MapStat
    {
        /// <summary>
        /// Название карты
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        /// Изображение карты
        /// </summary>
        public byte[] MapImage { get; set; }

        /// <summary>
        /// Количество сыгранных игр на карте
        /// </summary>
        public double CountGame { get; set; }

        /// <summary>
        /// % выпадение карт без использования подношения
        /// </summary>
        public double FalloutMapRandom { get; set; }

        /// <summary>
        /// % выпадение карт с использованием подношений
        /// </summary>
        public double FalloutMapOffering { get; set; }

        /// <summary>
        /// % убийств на карте
        /// </summary>
        public double KillRateMap { get; set; }

        /// <summary>
        /// % побега выживших с карты
        /// </summary>
        public double EscapeRateMap { get; set; }

        /// <summary>
        /// % побед на карте
        /// </summary>
        public double WinRateMap { get; set; }

       
    }
}

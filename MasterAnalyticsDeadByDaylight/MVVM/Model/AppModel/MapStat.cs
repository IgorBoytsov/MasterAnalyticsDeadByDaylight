﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class MapStat
    {

        public int Index { get; set; }

        /// <summary>
        /// Название карты
        /// </summary>
        public int idMap { get; set; }

        /// <summary>
        /// Название карты
        /// </summary>
        public string MapName { get; set; }

        /// <summary>
        /// Игровое измерение, где находится карта
        /// </summary>
        public string MapMeasurement { get; set; }

        /// <summary>
        /// id измерение, где находится карта
        /// </summary>
        public int idMapMeasurement { get; set; }

        /// <summary>
        /// Изображение карты
        /// </summary>
        public byte[] MapImage { get; set; }

        /// <summary>
        /// Количество сыгранных игр на карте
        /// </summary>
        public int CountGame { get; set; }

        /// <summary>
        /// Количество сыгранных игр на карте
        /// </summary>
        public double PickRateMap { get; set; }

        /// <summary>
        /// Выпадение карт без использования подношения
        /// </summary>
        public int FalloutMapRandom { get; set; }

        /// <summary>
        /// Выпадение карт с использованием подношений
        /// </summary>
        public int FalloutMapOffering { get; set; }

        /// <summary>
        /// % выпадение карт без использования подношения
        /// </summary>
        public double FalloutMapRandomPercent { get; set; }

        /// <summary>
        /// % выпадение карт с использованием подношений
        /// </summary>
        public double FalloutMapOfferingPercent { get; set; }

        /// <summary>
        /// AVG убийств на карте
        /// </summary>
        public double KillRateMap { get; set; }

        /// <summary>
        /// % убийств на карте
        /// </summary>
        public double KillRateMapPercent { get; set; }

        /// <summary>
        /// % побега выживших с карты
        /// </summary>
        public double EscapeRateMap { get; set; }

        /// <summary>
        /// % побед на карте
        /// </summary>
        public double WinRateMapPercent { get; set; }

        /// <summary>
        /// Количество победных игр на карте
        /// </summary>
        public int WinRateMap { get; set; }
    }
}

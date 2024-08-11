using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    class SurvivorStat
    {
        /// <summary>
        /// Имя выжившего
        /// </summary>
        public string SurvivorName { get; set; }

        /// <summary>
        /// Портрет выжившего
        /// </summary>
        public byte[] SurvivorImage { get; set; }

        /// <summary>
        /// Количество выживших
        /// </summary>
        public int SurvivorCount { get; set; }

        /// <summary>
        /// Пикрейт выжившего, как часто игрок играет за данного персонажа, в %
        /// </summary>
        public double SurvivorPickRate { get; set; }

        /// <summary>
        /// Количество побега с карт выжившего, в %
        /// </summary>
        public double SurvivorEscapeCount { get; set; }

        /// <summary>
        /// % побега с карт выжившего, в %
        /// </summary>
        public double SurvivorEscapePercentage { get; set; }

        /// <summary>
        /// % Анонимных игроков на выжавшем 
        /// </summary>
        public double SurvivorAnonymousModeCount { get; set; }

        /// <summary>
        /// % Анонимных игроков на выжавшем 
        /// </summary>
        public double SurvivorAnonymousModePercentage { get; set; }

        /// <summary>
        /// Средний престиж на выжившем
        /// </summary>
        public double SurvivorAVGPrestige { get; set; }

        /// <summary>
        /// Количество левеющих игроков на выжившем
        /// </summary>
        public double SurvivorBotCount { get; set; }

        /// <summary>
        /// % Левеющих игроков на выжившем
        /// </summary>
        public double SurvivorBotPercentage { get; set; }
    }
}

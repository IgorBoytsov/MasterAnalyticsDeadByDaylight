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
        /// Пикрейт выжившего, как часто игрок играет за данного персонажа, в %
        /// </summary>
        public double SurvivorPickRate { get; set; }

        /// <summary>
        /// % побега с карты выжившего, в %
        /// </summary>
        public double SurvivorEscapeRate { get; set; } 
        
        /// <summary>
        /// Частота встречаемости персонажа в матчах, в %
        /// </summary>
        public double SurvivorOccurrenceRate { get; set; }
    }
}

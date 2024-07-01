using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    class PerkStat
    {
        /// <summary>
        /// Названия перка
        /// </summary>
        public string PerkName { get; set; }

        /// <summary>
        /// Изображение навыка
        /// </summary>
        public byte[] PerkImage { get; set; }

        /// <summary>
        /// Пикрейт перка, как часто встречается перк, в %
        /// </summary>
        public double PerkPickRate { get; set; }
    }
}

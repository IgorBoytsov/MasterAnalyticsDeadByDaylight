using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    class Settings
    {
        #region Хранение настроек для окна "Добавление матча (AddMatchWindow)"

        /// <summary>
        /// Выбирать ли при выборе билда аддоны, или нет. 
        /// </summary>
        public bool ConsiderAddon {  get; set; }

        /// <summary>
        /// Выбирать ли при выборе билда киллера, или нет. 
        /// </summary>
        public bool ConsiderKiller { get; set; }

        #endregion

        #region Хранение настроек для окна "Резервных копий данных (DataBackupWindow)"

        /// <summary>
        /// Путь к папке для сохранение резервных данных
        /// </summary>
        public string FolderPath { get; set; }

        #endregion
    }
}

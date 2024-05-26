using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class Perk
    {
        public int IdPerk { get; set; }

        public int IdCharacter { get; set; }

        public string PerkName { get; set; }

        public byte[] PerkImage { get; set; }

        public string PerkDescription { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class Character
    {
        public int IdCharacter {  get; set; }

        public string NameCharacter { get; set; }

        public byte[] ImageCharacter { get; set; }
    }
}

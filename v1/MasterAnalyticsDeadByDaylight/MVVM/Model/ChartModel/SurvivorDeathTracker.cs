using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.ChartModel
{
    public class SurvivorDeathTracker
    {
        public string SurvivorName { get; set;}

        public byte[] SurvivorImage { get; set; }

        public int CountDeathHook { get; set;}

        public int CountDeathMemento {  get; set; }

        public int CountDeathKillersAbility {  get; set; }

        public int CountDeathGround {  get; set; }

        public int CountEscaped {  get; set; }

        public int TotalDead { get; set; }
    }
}

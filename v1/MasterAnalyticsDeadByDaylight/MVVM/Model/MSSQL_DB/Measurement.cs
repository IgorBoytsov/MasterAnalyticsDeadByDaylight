using System;
using System.Collections.Generic;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;

public partial class Measurement
{
    public int IdMeasurement { get; set; }

    public string MeasurementName { get; set; }

    public string MeasurementDescription { get; set; }

    public virtual ICollection<Map> Maps { get; set; } = new List<Map>();
}

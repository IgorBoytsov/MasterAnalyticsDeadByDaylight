using System;
using System.Collections.Generic;

namespace DBDAnalytics.Infrastructure.Models;

public partial class Measurement
{
    public int IdMeasurement { get; set; }

    public string MeasurementName { get; set; } = null!;

    public string? MeasurementDescription { get; set; }

    public virtual ICollection<Map> Maps { get; set; } = new List<Map>();
}

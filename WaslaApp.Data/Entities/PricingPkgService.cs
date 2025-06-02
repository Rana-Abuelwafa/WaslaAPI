using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class PricingPkgService
{
    public decimal id { get; set; }

    public int? package_id { get; set; }

    public int? service_id { get; set; }

    public string? service_name { get; set; }
}

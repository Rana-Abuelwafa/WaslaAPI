using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class PricingPkgFeature
{
    public decimal id { get; set; }

    public int? package_id { get; set; }

    public string? feature_name { get; set; }

    public string? feature_desc { get; set; }

    public string? lang_code { get; set; }

    public bool? active { get; set; }
}

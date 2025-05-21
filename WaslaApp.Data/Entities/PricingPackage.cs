using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class PricingPackage
{
    public int package_id { get; set; }

    public string? package_name { get; set; }

    public string? package_desc { get; set; }

    public string? package_details { get; set; }

    public decimal? package_sale_price { get; set; }

    public decimal? package_price { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? end_date { get; set; }

    public string? lang_code { get; set; }

    public bool? active { get; set; }
}

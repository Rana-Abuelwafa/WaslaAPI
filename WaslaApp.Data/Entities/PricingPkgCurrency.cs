using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class PricingPkgCurrency
{
    public int id { get; set; }

    public string? curr_code { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? end_date { get; set; }

    public decimal? discount_amount { get; set; }

    /// <summary>
    /// 1 = percentage
    /// 2 = amount
    /// </summary>
    public short? discount_type { get; set; }

    public decimal? package_price { get; set; }

    public decimal? package_sale_price { get; set; }

    public int? package_id { get; set; }

    public bool? active { get; set; }
}

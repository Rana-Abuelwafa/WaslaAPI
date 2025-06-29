using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class service_package_price
{
    public int id { get; set; }

    public int service_package_id { get; set; }

    public string curr_code { get; set; } = null!;

    public decimal package_price { get; set; }

    public decimal package_sale_price { get; set; }

    public decimal discount_amount { get; set; }

    public int? discount_type { get; set; }

    public virtual service_package service_package { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class packagesdetailswithservice
{
    public int? service_package_id { get; set; }

    public int? service_id { get; set; }

    public int? package_id { get; set; }

    public string? package_code { get; set; }

    public string? package_name { get; set; }

    public string? package_desc { get; set; }

    public string? package_details { get; set; }

    public short? order { get; set; }

    public bool? is_recommend { get; set; }

    public bool? is_custom { get; set; }

    public string? service_code { get; set; }

    public string? service_name { get; set; }

    public string? service_desc { get; set; }

    public string? lang_code { get; set; }

    public bool? active { get; set; }

    public string? curr_code { get; set; }

    public decimal? package_sale_price { get; set; }

    public decimal? package_price { get; set; }

    public decimal? discount_amount { get; set; }

    public int? discount_type { get; set; }
}

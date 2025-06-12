using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ApplyTax
{
    public int tax_id { get; set; }

    public string? tax_code { get; set; }

    public string? tax_name { get; set; }

    public decimal? tax_amount { get; set; }
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ApplyTax
{
    public int tax_id { get; set; }

    public string? tax_code { get; set; }

    public string? tax_name { get; set; }

    public decimal? tax_amount { get; set; }

    public char? tax_sign { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

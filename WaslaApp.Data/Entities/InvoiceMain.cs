using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class InvoiceMain
{
    public decimal invoice_id { get; set; }

    public string? client_id { get; set; }

    public string? invoice_code_auto { get; set; }

    public string? invoice_code { get; set; }

    public DateTime? invoice_date { get; set; }

    public bool? active { get; set; }

    public string? client_email { get; set; }

    public string? client_name { get; set; }

    public decimal? total_price { get; set; }

    public decimal? discount { get; set; }

    public string? curr_code { get; set; }

    /// <summary>
    /// 1 = pending
    /// 2=checkout
    /// 3=confirmed
    /// </summary>
    public short? status { get; set; }

    public decimal? grand_total_price { get; set; }

    public decimal? copoun_id { get; set; }

    public int? tax_id { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

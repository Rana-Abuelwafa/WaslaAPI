using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class clientinvoiceswithdetail
{
    public int? productId { get; set; }

    public string? client_id { get; set; }

    public bool? active { get; set; }

    public decimal? invoice_id { get; set; }

    public int? package_id { get; set; }

    public int? service_package_id { get; set; }

    public string? client_email { get; set; }

    public string? client_name { get; set; }

    public decimal? copoun_id { get; set; }

    public string? curr_code { get; set; }

    public decimal? discount { get; set; }

    public decimal? grand_total_price { get; set; }

    public string? invoice_code { get; set; }

    public string? invoice_code_auto { get; set; }

    public DateTime? invoice_date { get; set; }

    public short? status { get; set; }

    public int? tax_id { get; set; }

    public decimal? total_price { get; set; }

    public decimal? tax_amount { get; set; }

    public string? tax_code { get; set; }

    public string? tax_name { get; set; }

    public char? tax_sign { get; set; }

    public string? copoun { get; set; }

    public short? copoun_discount_type { get; set; }

    public decimal? copoun_discount_value { get; set; }

    public decimal? client_service_id { get; set; }
}

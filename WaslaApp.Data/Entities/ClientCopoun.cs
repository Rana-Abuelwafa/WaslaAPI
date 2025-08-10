using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ClientCopoun
{
    public decimal id { get; set; }

    public string? copoun { get; set; }

    public DateOnly? start_date { get; set; }

    public DateOnly? end_date { get; set; }

    public string? client_id { get; set; }

    public decimal? discount_value { get; set; }

    /// <summary>
    /// 1 = percentage
    /// 2 = amount
    /// </summary>
    public short? discount_type { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

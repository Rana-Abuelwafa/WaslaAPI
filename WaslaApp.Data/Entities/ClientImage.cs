using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ClientImage
{
    public decimal id { get; set; }

    public string client_id { get; set; } = null!;

    public string? img_name { get; set; }

    public string? img_path { get; set; }

    /// <summary>
    /// 1 for profile
    /// </summary>
    public int? type { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

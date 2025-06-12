using System;
using System.Collections.Generic;

namespace Wasla_App.Entities;

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
}

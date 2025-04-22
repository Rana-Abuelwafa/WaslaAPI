using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ClientBrand
{
    public decimal id { get; set; }

    public string client_Id { get; set; } = null!;

    public string? brand_name { get; set; }

    public string? brand_type { get; set; }

    public string? brand_desc { get; set; }
}

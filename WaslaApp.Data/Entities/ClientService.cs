using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class ClientService
{
    public int? productId { get; set; }

    public string? client_id { get; set; }

    public decimal id { get; set; }

    public int? package_id { get; set; }

    public decimal invoice_id { get; set; }
}

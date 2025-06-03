using System;
using System.Collections.Generic;

namespace Wasla_App.Entities;

public partial class ClientCopoun
{
    public decimal id { get; set; }

    public string? copoun { get; set; }

    public DateOnly? start_date { get; set; }

    public DateOnly? end_date { get; set; }

    public string? client_id { get; set; }
}

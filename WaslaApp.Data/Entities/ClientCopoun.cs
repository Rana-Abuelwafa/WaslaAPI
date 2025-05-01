using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaslaApp.Data.Entities;

public partial class ClientCopoun
{
    public decimal id { get; set; }

    public string? copoun { get; set; }
    [DataType(DataType.Date)]
    public DateOnly? start_date { get; set; }
    [DataType(DataType.Date)]
    public DateOnly? end_date { get; set; }

    public string? client_id { get; set; }
}

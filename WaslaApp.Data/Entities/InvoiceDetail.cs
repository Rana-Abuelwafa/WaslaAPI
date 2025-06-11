using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class InvoiceDetail
{
    public decimal id { get; set; }

    public decimal? invoice_id { get; set; }

    public int? package_id { get; set; }

    public int? service_id { get; set; }
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class Service
{
    public int productId { get; set; }

    public string? productName { get; set; }

    public int? productParent { get; set; }

    public string? product_desc { get; set; }

    public bool? active { get; set; }

    public string? lang_code { get; set; }

    public bool? leaf { get; set; }

    public decimal? price { get; set; }
}

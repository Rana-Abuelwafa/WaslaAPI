using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class service_translation
{
    public int id { get; set; }

    public int service_id { get; set; }

    public string lang_code { get; set; } = null!;

    public string? productname { get; set; }

    public string? product_desc { get; set; }

    public virtual main_service service { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class main_feature
{
    public int id { get; set; }

    public string? feature_code { get; set; }

    public string? feature_default_name { get; set; }

    public bool? active { get; set; }
}

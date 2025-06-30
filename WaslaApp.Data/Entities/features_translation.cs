using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class features_translation
{
    public int id { get; set; }

    public int feature_id { get; set; }

    public string? feature_name { get; set; }

    public string? feature_description { get; set; }

    public string? lang_code { get; set; }
}

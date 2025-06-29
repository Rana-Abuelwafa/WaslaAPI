using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class package_translation
{
    public int id { get; set; }

    public int package_id { get; set; }

    public string lang_code { get; set; } = null!;

    public string package_name { get; set; } = null!;

    public string? package_desc { get; set; }

    public string? package_details { get; set; }

    public virtual package package { get; set; } = null!;
}

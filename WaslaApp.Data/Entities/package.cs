using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class package
{
    public int id { get; set; }

    public string package_code { get; set; } = null!;

    public bool? is_recommend { get; set; }

    public bool? is_custom { get; set; }

    public bool? active { get; set; }

    public string? default_name { get; set; }

    public short? order { get; set; }

    public virtual ICollection<package_translation> package_translations { get; set; } = new List<package_translation>();

    public virtual ICollection<service_package> service_packages { get; set; } = new List<service_package>();
}

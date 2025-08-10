using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class main_service
{
    public int id { get; set; }

    public string service_code { get; set; } = null!;

    public bool? active { get; set; }

    public string? default_name { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }

    public virtual ICollection<service_package> service_packages { get; set; } = new List<service_package>();

    public virtual ICollection<service_translation> service_translations { get; set; } = new List<service_translation>();
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class service_package
{
    public int id { get; set; }

    public int service_id { get; set; }

    public int package_id { get; set; }

    public bool? is_recommend { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }

    public virtual package package { get; set; } = null!;

    public virtual main_service service { get; set; } = null!;

    public virtual ICollection<service_package_price> service_package_prices { get; set; } = new List<service_package_price>();
}

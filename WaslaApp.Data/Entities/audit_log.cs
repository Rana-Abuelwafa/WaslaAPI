using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class audit_log
{
    public long id { get; set; }

    public string schema_name { get; set; } = null!;

    public string table_name { get; set; } = null!;

    public string operation { get; set; } = null!;

    public string record_pk { get; set; } = null!;

    public DateTime? changed_at { get; set; }

    public string? changed_by { get; set; }
}

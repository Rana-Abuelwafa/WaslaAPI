using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class Main_RegistrationQuestion
{
    public int ques_id { get; set; }

    public string? ques_title_default { get; set; }

    public int? order { get; set; }

    public bool? active { get; set; }

    public string? ques_type { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class RegistrationAnswer
{
    public int id { get; set; }

    public int ques_id { get; set; }

    public string client_id { get; set; } = null!;

    public string? answer { get; set; }

    public string? lang_code { get; set; }

    public DateTime? created_at { get; set; }

    public DateTime? updated_at { get; set; }

    public string? created_by { get; set; }
}

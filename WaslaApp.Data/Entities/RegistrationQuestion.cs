using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class RegistrationQuestion
{
    public int ques_id { get; set; }

    public string? ques_title { get; set; }

    public string? ques_type { get; set; }

    public string? lang_code { get; set; }

    public int? order { get; set; }
}

using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class registrationqueswithlang
{
    public int? order { get; set; }

    public bool? active { get; set; }

    public int? ques_id { get; set; }

    public string? ques_title_default { get; set; }

    public string? ques_type { get; set; }

    public int? id { get; set; }

    public string? lang_code { get; set; }

    public string? ques_title { get; set; }
}

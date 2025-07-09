using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class RegistrationQuestions_Translation
{
    public int id { get; set; }

    public int ques_id { get; set; }

    public string? ques_title { get; set; }

    public string? lang_code { get; set; }
}

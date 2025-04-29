using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaslaApp.Data.Entities;

public partial class ClientProfile
{
    public decimal profile_id { get; set; }

    public string client_id { get; set; } = null!;

    public string? client_name { get; set; }

    public string? client_email { get; set; }

    public string? phone_number { get; set; }

    public string? nation { get; set; }

    public string? gender { get; set; }

    public string? lang { get; set; }

    public string? pay_code { get; set; }

    public string? fb_link { get; set; }

    public string? twitter_link { get; set; }

    public DateOnly? client_birthday { get; set; }
    [NotMapped]
    public string? client_birthdayStr { get; set; }
}

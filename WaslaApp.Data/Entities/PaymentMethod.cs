﻿using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class PaymentMethod
{
    public int pay_id { get; set; }

    public string? pay_code { get; set; }

    public string? pay_name { get; set; }

    public string? notes { get; set; }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public  class PricingPkgCurrencyCast : PricingPkgCurrency
    {
        public string? start_dateStr { get; set; }

        public string? end_dateStr { get; set; }
    }
}

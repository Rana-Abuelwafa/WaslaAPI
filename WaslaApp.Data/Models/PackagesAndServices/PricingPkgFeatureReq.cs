﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PricingPkgFeatureReq
    {
        public int package_id { get; set; }
        public bool? active { get; set; }
        public string? lang_code { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models
{
    public class PricingPackageCast : PricingPackage
    {
        public string? start_dateStr { get; set; }

        public string? end_dateStr { get; set; }
    }
}

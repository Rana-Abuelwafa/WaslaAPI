using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models
{
    public class PricingPackageCast
    {
        public int? package_id { get; set; }

        public string? package_name { get; set; }

        public string? package_desc { get; set; }

        public string? package_details { get; set; }

        public decimal? package_sale_price { get; set; }

        public decimal? package_price { get; set; }
        public string? curr_code { get; set; }
        public List<ServicesWithPkg>? services { get; set; }
    }
}

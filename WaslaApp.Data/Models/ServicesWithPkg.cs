using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models
{
    public class ServicesWithPkg
    {
   
        public int? service_id { get; set; }

        public string? service_name { get; set; }

        public List<PricingPackageCast>? pkgs { get; set; }
    }
}

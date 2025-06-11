using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PricingPackageReq
    {
        public int service_id { get; set; }
        public string? curr_code { get; set; }
        public string? lang_code { get; set; }
        public bool? active { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PricingPackageWithService : PricingPackage
    {
        public string? service_name { get; set; }
        public string? service_code { get; set; }
    }
}

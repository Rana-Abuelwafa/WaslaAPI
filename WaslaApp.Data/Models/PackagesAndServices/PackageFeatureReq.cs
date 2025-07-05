using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PackageFeatureReq
    {
        public int? service_package_id { get; set; }
        public string? lang_code { get; set; }
    }
}

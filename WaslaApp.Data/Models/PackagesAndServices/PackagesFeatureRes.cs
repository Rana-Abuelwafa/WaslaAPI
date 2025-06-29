using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PackagesFeatureRes
    {
        public int id { get; set; }

        public int? package_id { get; set; }

        public int? feature_id { get; set; }
        public string? feature_code { get; set; }
        public string? feature_default_name { get; set; }
    }
}

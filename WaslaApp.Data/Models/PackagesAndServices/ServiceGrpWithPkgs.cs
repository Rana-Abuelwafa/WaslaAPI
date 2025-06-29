using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class ServicePkgsWithDetails
    {
        public short? order { get; set; }
        public int service_package_id { get; set; }
        public string service_code { get; set; } = null!;
        public int service_id { get; set; }
        public int package_id { get; set; }
        public string package_code { get; set; } = null!;
        public bool? is_recommend { get; set; }
        public bool? is_custom { get; set; }
        public string? package_default_name { get; set; }
        public string? service_default_name { get; set; }
    }

    public class ServiceGrpWithPkgs
    {
        public string service_code { get; set; } = null!;
        public int service_id { get; set; }
        public string? service_default_name { get; set; }
        public List<ServicePkgsWithDetails> pkgs { get; set; }
    }
}

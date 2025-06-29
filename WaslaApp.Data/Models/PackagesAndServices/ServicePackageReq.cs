using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class ServicePackageReq
    {
        public int id { get; set; }
        public int service_id { get; set; }

        public int package_id { get; set; }
    }
}

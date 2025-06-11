using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public  class PackageServiceAssignReq
    {
        public int package_id { get; set; }
        public List<int> product_ids { get; set; } = new List<int>();
    }
}

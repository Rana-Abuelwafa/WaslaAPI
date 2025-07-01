using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PackagePriceSaveReq
    {
        public int id { get; set; }

        public int service_package_id { get; set; }

        public string curr_code { get; set; } = null!;

        public decimal package_price { get; set; }

        public decimal package_sale_price { get; set; }

        public decimal discount_amount { get; set; }

        public int? discount_type { get; set; }
        public bool delete { get; set; }
    }
}

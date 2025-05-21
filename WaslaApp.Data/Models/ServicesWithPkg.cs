using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models
{
    public class ServicesWithPkg
    {
        public decimal servicepkg_id { get; set; }

        public int? package_id { get; set; }

        public int? service_id { get; set; }
        public string? service_name { get; set; }

        public string? package_name { get; set; }

        public string? package_desc { get; set; }

        public string? package_details { get; set; }

        public decimal? package_sale_price { get; set; }

        public decimal? package_price { get; set; }
        public string? curr_code { get; set; }

        public decimal? discount_amount { get; set; }

        /// <summary>
        /// 1 = percentage
        /// 2 = amount
        /// </summary>
        public short? discount_type { get; set; }
    }
}

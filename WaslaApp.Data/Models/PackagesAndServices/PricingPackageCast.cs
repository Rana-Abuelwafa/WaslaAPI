using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PricingPackageCast 
    {
        public int package_id { get; set; }

        public string? package_name { get; set; }

        public string? package_desc { get; set; }

        public string? package_details { get; set; }

        public decimal? package_sale_price { get; set; }

        public decimal? package_price { get; set; }

        public DateTime? start_date { get; set; }

        public DateTime? end_date { get; set; }

        public string? lang_code { get; set; }

        public bool? active { get; set; }

        public int? order { get; set; }

        public string? curr_code { get; set; }

        public decimal? discount_amount { get; set; }

        /// <summary>
        /// 1 = percentage
        /// 2 = amount
        /// </summary>
        public short? discount_type { get; set; }

        public int service_id { get; set; }

        public string? package_code { get; set; }

        public bool? is_recommend { get; set; }

        public bool? is_custom { get; set; }
        public string? service_name { get; set; }
        public string? service_code { get; set; }
        public string? start_dateStr { get; set; }
        public string? end_dateStr { get; set; }
        public bool? isSelected { get; set; }
        public int? service_package_id { get; set; }
        //public List<PricingPkgFeature> features { get; set; }
        public List<PackagesFeatureRes> features { get; set; }
        
    }
}

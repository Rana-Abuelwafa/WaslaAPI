using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.reports
{
    public class SummaryServiceResponse
    {

        public string? invoice_date { get; set; }
        public decimal? tax_amount { get; set; }
        public decimal? copoun_discount { get; set; }
        public decimal? total_price { get; set; }

        public decimal? discount { get; set; }

        public string? curr_code { get; set; }
        public decimal? grand_total_price { get; set; }
        public string? service_name { get; set; }
        public int? service_id { get; set; }

        public string? invoice_code { get; set; }
        public short? status { get; set; }
        public string? client_email { get; set; }
        public string? client_name { get; set; }
        public int? service_package_id { get; set; }

    }

    public class SummaryServiceResponseGrp
    {
        public decimal? NetValTotal { get; set; }
        public decimal? GrandTotalVat { get; set; }
        public decimal? GrandTotalAmount { get; set; }
        public decimal? GrandTotalDiscount { get; set; }
        public string? currency_code { get; set; }
        public string? service_name { get; set; }
    }
  
}

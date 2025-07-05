using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class InvoiceReq
    {
        public string? lang_code { get; set; }
        public string? curr_code { get; set; }
        public decimal? discount_amount { get; set; }
        public decimal? package_sale_price { get; set; }
        public decimal? package_price { get; set; }
        public string? service_name { get; set; }
        public string? service_code { get; set; }
        public string? package_code { get; set; }
        public string? package_name { get; set; }
        public int? productId { get; set; }
        public int? package_id { get; set; }
        public bool? is_custom { get; set; }
        public int? service_package_id { get; set; }
    }
}

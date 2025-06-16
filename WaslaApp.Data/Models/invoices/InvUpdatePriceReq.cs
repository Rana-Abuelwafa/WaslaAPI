using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class InvUpdatePriceReq
    {
        public decimal? copoun_id { get; set; }
        public decimal invoice_id { get; set; }
        public decimal? copoun_discount { get; set; }
        public int tax_id { get; set; }
        public decimal? deduct_amount { get; set; }
        public decimal? total_price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class CheckoutReq
    {
        public short? status { get; set; }
        public decimal? grand_total_price { get; set; }
        public decimal? copoun_id { get; set; }
        public decimal invoice_id { get; set; }
        public decimal? copoun_discount { get; set; }

    }
}

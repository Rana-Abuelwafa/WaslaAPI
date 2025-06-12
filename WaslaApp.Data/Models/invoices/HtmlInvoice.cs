using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.invoices
{
    public class HtmlInvoice 
    {
        public string? user {  get; set; }
        public string? contact { get; set; }
        public string? address { get; set; }
        public string? InvoiceNo { get; set; }
        public string? IssuedDate { get; set; }
        public decimal? SubTtotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Total { get; set; }
        public List<InvoiceReq> services { get; set; }
    }
}

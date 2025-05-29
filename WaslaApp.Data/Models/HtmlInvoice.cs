using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models
{
    public class HtmlInvoice
    {
        public string? user {  get; set; }
        public string? contact { get; set; }
        public string? address { get; set; }
        public string? InvoiceNo { get; set; }
        public string? IssuedDate { get; set; }
        public string? SubTtotal { get; set; }
        public string? Discount { get; set; }
        public string? Total { get; set; }
        public List<Service> services { get; set; }
    }
}

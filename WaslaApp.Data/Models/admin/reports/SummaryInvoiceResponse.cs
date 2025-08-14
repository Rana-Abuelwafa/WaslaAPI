using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.invoices;

namespace WaslaApp.Data.Models.admin.reports
{
    public class SummaryInvoiceResponse
    {
        public decimal? NetValTotal { get; set; }
        public decimal? GrandTotalVat { get; set; }
        public decimal? GrandTotalAmount { get; set; }
        public decimal? GrandTotalDiscount { get; set; }
        public string? currency_code { get; set; }
        public List<clientinvoiceswithdetail> invoices { get; set; }
    }

  
}

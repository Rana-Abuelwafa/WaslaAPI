using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class ClientInvoiceReq
    {
        /// 1 = pending
        /// 2=checkout
        /// 3=confirmed
        public int? status { get; set; }
        public bool? active { get; set; }
        public string? lang_code { get; set; }
       // public string? curr_code { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Models.global;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class InvoiceResponse : ResponseCls
    {
        public HtmlInvoice invoice {  get; set; }
    }
}

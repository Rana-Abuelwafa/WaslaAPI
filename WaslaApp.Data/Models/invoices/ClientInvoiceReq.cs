using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class ClientInvoiceReq
    {
        public string? client_id {  get; set; }
        public bool? active { get; set; }

    }
}

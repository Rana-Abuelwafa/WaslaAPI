using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.Accounting
{
    public class ChangeInvoiceStatusReq
    {
        public short status {  get; set; }
        public string? client_id { get; set; }
        public decimal? invoice_id { get; set; }
    }
}

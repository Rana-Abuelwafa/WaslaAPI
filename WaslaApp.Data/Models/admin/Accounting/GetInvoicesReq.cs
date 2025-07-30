using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.Accounting
{
    public class GetInvoicesReq
    {
        public string? date_from { get; set; }
        public string? date_to { get; set; }
        public string? client_email { get; set; }
        public int? status { get; set; }
        public bool? active { get; set; }
        public string? invoice_code { get; set; }
        public string? lang_code { get; set; }
    }
}

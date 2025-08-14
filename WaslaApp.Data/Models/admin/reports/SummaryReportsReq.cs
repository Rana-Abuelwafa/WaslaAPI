using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.reports
{
    public class SummaryReportsReq
    {
        public int month {  get; set; }
        public string? client_email { get; set; }
        public int service_id { get; set; }
        public int package_id { get; set; }
    }
}

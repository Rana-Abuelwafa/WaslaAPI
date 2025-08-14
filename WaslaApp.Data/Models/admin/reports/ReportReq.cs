using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.reports
{
    public class ReportReq
    {
        public string? date_from { get; set; }
        public string? date_to { get; set; }

        public int? report_id { get; set; }
    }
}

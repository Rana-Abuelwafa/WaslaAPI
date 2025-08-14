using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.reports
{
    public class SummaryServiceResponseCurr
    {
        public string? currency_code { get; set; }
        public List<SummaryServiceResponseGrp> result { get; set; }
    }
}

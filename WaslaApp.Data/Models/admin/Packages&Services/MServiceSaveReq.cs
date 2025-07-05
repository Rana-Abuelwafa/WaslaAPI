using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.Packages_Services
{
    public class MServiceSaveReq
    {
        public int id { get; set; }

        public string service_code { get; set; } = null!;

        public bool? active { get; set; }

        public string? default_name { get; set; }
    }
}

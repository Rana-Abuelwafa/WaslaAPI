using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.admin.Packages_Services
{
    public class ServiceTranslationSaveReq
    {
        public int id { get; set; }

        public int service_id { get; set; }

        public string lang_code { get; set; } = null!;

        public string? productname { get; set; }

        public string? product_desc { get; set; }
        public bool? delete { get; set; }
    }
}

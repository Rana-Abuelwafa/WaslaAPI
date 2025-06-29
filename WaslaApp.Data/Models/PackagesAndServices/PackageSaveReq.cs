using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.PackagesAndServices
{
    public class PackageSaveReq
    {
        public int id { get; set; }

        public string package_code { get; set; } = null!;

        public bool? is_recommend { get; set; }

        public bool? is_custom { get; set; }

        public bool? active { get; set; }

        public string? default_name { get; set; }
        public short? order { get; set; }
    }
}

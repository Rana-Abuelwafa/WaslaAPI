using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.profile
{
    public class ClientCopounCast : ClientCopoun
    {
        public bool? valid { get; set; }
        public string? msg { get; set; }
        public string? start_dateStr { get; set; }

        public string? end_dateStr { get; set; }
    }
}

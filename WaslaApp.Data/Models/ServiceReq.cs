using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models
{
    public class ServiceReq
    {
        public int parent {  get; set; }
        public bool active { get; set; }
        public string? lang { get; set; }
    }
}

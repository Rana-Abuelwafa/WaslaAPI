using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models
{
    public class Service_Tree : Service
    {
        public bool isSelected { get; set; }
        public decimal clientServiceId { get; set; }
        public List<Service_Tree>? children { get; set; }
    }
}

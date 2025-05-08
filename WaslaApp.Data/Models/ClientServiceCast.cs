using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models
{
    public class ClientServiceCast :ClientService
    {
        public bool isSelected { get; set; }
    }
}

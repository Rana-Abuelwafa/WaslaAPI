using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models
{
    public class ResponseCls 
    {
        public bool success { get; set; }
        public string? errors { get; set; }
        public decimal idOut { get; set; }

    }
}

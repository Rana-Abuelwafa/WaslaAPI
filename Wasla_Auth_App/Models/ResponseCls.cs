using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla_Auth_App.Models
{
    public class ResponseCls 
    {
        public bool success { get; set; }
        public string? errors { get; set; }
        public string? message { get; set; }

    }
}

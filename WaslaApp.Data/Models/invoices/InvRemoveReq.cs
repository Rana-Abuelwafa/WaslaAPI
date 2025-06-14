﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data.Models.invoices
{
    public class InvRemoveReq
    {
        public bool? active { get; set; }
 
        public decimal invoice_id { get; set; }

        public int package_id { get; set; }
        public int service_id { get; set; }
    }
}

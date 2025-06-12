﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.invoices
{
    public class ClientInvoiceResponse 
    {
        public decimal invoice_id { get; set; }

       // public decimal? tax_amount { get; set; }
       // public string? copoun { get; set; }
        //public decimal? copoun_discount { get; set; }
        public decimal? total_price { get; set; }

        public decimal? discount { get; set; }

        public string? curr_code { get; set; }
        public decimal? grand_total_price { get; set; }
        public decimal? package_sale_price { get; set; }
        public decimal? package_price { get; set; }
        public string? service_name { get; set; }
        public string? package_name { get; set; }
        public int? service_id { get; set; }
        public int? package_id { get; set; }
        public string? package_desc { get; set; }
        public string? invoice_code_auto { get; set; }

        public string? invoice_code { get; set; }
        public string? package_details { get; set; }
        public short? status { get; set; }
    }

    public class ClientInvoiceGrp
    {
        public decimal invoice_id { get; set; }
        public short? status { get; set; }
        //public decimal? tax_amount { get; set; }
        //public string? copoun { get; set; }
        //public decimal? copoun_discount { get; set; }
        public decimal? total_price { get; set; }
        public string? invoice_code_auto { get; set; }

        public string? invoice_code { get; set; }
        public decimal? discount { get; set; }

        public string? curr_code { get; set; }
        public decimal? grand_total_price { get; set; }
        public List<ClientInvoiceResponse> pkgs { get; set; }
    }
}

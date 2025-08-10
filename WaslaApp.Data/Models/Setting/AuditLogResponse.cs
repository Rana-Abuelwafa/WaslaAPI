using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.Setting
{
    public class AuditLogResponse
    {
        public int totalPages { get; set; }
        public List<AuditLogCls>? result { get; set; }
    }
    public class AuditLogCls : audit_log
    {
        public string? changed_atStr { get; set; }
    }
}

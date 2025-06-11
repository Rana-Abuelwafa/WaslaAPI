using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.profile
{
    public class ClientProfileCast : ClientProfile
    {
        [NotMapped]
        public string? client_birthdayStr { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.profile
{
    public class QuesWithAnswers : RegistrationQuestion
    {
        public string? answer { get; set; }
        public string client_id { get; set; } = null!;
    }
}

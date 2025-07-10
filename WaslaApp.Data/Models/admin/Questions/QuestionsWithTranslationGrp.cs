using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.admin.Questions
{
   
    public class QuestionsWithTranslationGrp
    {
        public int? ques_id { get; set; }

        public short? ques_type { get; set; }

        public string? ques_title_default { get; set; }

        public int? order { get; set; }
        public bool? active { get; set; }
        public List<registrationqueswithlang> questions { get; set; }
        
    }
}

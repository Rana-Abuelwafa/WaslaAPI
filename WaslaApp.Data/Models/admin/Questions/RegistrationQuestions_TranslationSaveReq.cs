using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.admin.Questions
{
    public class RegistrationQuestions_TranslationSaveReq : RegistrationQuestions_Translation
    {
        public bool? delete { get; set; }
    }
}

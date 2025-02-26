using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace WaslaApp.Data
{
    public class WaslaDAO
    {
        private readonly wasla_client_dbContext _db;
        string htmlcontent = "<p>Dear customer, the form has been submitted to our \r\ndesigners for review and implementation, you will be \r\ncontacted to obtain further details.\r\n<br/>\r\n<br/>\r\n Thank you for choosing our services.<p/>";

        public  WaslaDAO(wasla_client_dbContext db) {
            _db = db;
        }

        #region "select"
        public async Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {

            try
            {
                return await _db.RegistrationQuestions.Where(wr => wr.lang_code == lang).ToListAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region "insert & update"
         public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string client_id)
        {
            int count = 0;
            RegsistrationQuesResponse response;
            try
            {
                foreach(RegistrationAnswer answer in lst)
                {
                    answer.client_id = client_id;
                    if (answer.id  == 0)
                    {
                        _db.RegistrationAnswers.Add(answer);
                    }
                    else
                    {
                        _db.RegistrationAnswers.Update(answer);
                    }
                   
                    
                    count++;
                }
                
                _db.SaveChanges();
                if (count == lst.Count)
                {
                    response = new RegsistrationQuesResponse { errors = null, success = true, WelcomeMsg = htmlcontent };
                }
                else
                {
                    response = new RegsistrationQuesResponse { errors = "Error in saving List Check Admin", success = false, WelcomeMsg = null };
                }
            }
            
            catch(Exception ex)
            {
                response = new RegsistrationQuesResponse { errors = ex.Message, success = false, WelcomeMsg = null };
            }
            return response;
        }
        #endregion

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;
using WaslaApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WaslaApp.Data
{
    public class WaslaDAO
    {
        private readonly MailSettingDao _mailSettingDao;
        private readonly wasla_client_dbContext _db;
        string htmlcontent = "<p>Dear customer, the form has been submitted to our \r\ndesigners for review and implementation, you will be \r\ncontacted to obtain further details.\r\n<br/>\r\n<br/>\r\n Thank you for choosing our services.<p/>";

        public  WaslaDAO(wasla_client_dbContext db, MailSettingDao mailSettingDao) {
            _db = db;
            _mailSettingDao = mailSettingDao;
        }

        #region "questions"

        #region "select"
        public async Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {

            try
            {
                if(lang.ToLower() == "all")
                {
                    return await _db.RegistrationQuestions.ToListAsync();
                }
                else
                {
                    return await _db.RegistrationQuestions.Where(wr => wr.lang_code == lang).ToListAsync();
                }
                
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region "insert & update"
         public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string client_id,string FullName,string email)
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
                    //send mail
                    try
                    {
                        MailData Mail_Data = new MailData { EmailToId = email, EmailToName = FullName, EmailSubject = "wasla activation mail" };
                        _mailSettingDao.SendMail(Mail_Data);
                    }
                    catch (Exception e)
                    {

                    }

                    //end send mail
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
         
        public ResponseCls saveQuestions(RegistrationQuestion ques)
        {
            ResponseCls response;
            try
            {
                if(ques.ques_id == 0)
                {
                    int maxId = _db.RegistrationQuestions.Max(d => d.ques_id);
                    ques.ques_id = maxId + 1;
                    _db.RegistrationQuestions.Add(ques);
                }
                else
                {
                    _db.Update(ques);
                }
                
                _db.SaveChanges();
                response = new ResponseCls {  success=true,errors=null};
            }
            catch(Exception ex)
            {
                response = new ResponseCls { success=false, errors = ex.Message};
            }
            return response;
        }


        public ResponseCls deleteQuestions(RegistrationQuestion ques)
        {
            ResponseCls response;
            try
            {
               
                    _db.Remove(ques);
                
                _db.SaveChanges();
                response = new ResponseCls { success = true, errors = null };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message };
            }
            return response;
        }

        #endregion

        #endregion

        #region "Profile"

        public async Task<List<PaymentMethod>> GetPaymentMethods()
        {

            try
            {               
                    return await _db.PaymentMethods.ToListAsync();
   
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ClientBrand>> GetClientBrands(string clientId)
        {
            try
            {
                return await _db.ClientBrands.Where(wr => wr.client_Id == clientId).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ClientProfile>> GetClientProfiles(string clientId)
        {
            try
            {
                return await _db.ClientProfiles.Where(wr => wr.client_id == clientId).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}

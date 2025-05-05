﻿using Mails_App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WaslaApp.Data
{
    public class WaslaDAO
    {
        private readonly MailSettingDao _mailSettingDao;
        private readonly wasla_client_dbContext _db;
        string htmlcontent = "<p>Dear customer, the form has been submitted to our \r\ndesigners for review and implementation, you will be \r\ncontacted to obtain further details.\r\n<br/>\r\n<br/>\r\n Thank you for choosing our services.<p/>";
        public WaslaDAO(wasla_client_dbContext db, MailSettingDao mailSettingDao)
        {
            _db = db;
            _mailSettingDao = mailSettingDao;
        }

        #region "questions"

        #region "select"
        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang)
        {
            try
            {
                var result = from ques in _db.RegistrationQuestions.Where(wr => wr.lang_code == lang)
                             join ans in _db.RegistrationAnswers.Where(wr => wr.client_id == clientId) on ques.ques_id equals ans.ques_id into quesWans
                             from m in quesWans.DefaultIfEmpty()
                             select new QuesWithAnswers
                             {
                                 ques_id = ques.ques_id,
                                 lang_code = ques.lang_code,
                                 client_id = m.client_id,
                                 answer = m.answer,
                                 order = ques.order,
                                 ques_title = ques.ques_title,
                                 ques_type = ques.ques_type

                             };
                //var result = _db.RegistrationQuestions.Where(wr => wr.lang_code == lang)
                //    .GroupJoin(
                //       _db.RegistrationAnswers.Where(wr => wr.client_id == clientId),
                //         ques => new { ques.ques_id },
                //         ans => new { ans.ques_id },
                //         (ques, ans) => new { ques, ans })
                //        .SelectMany(x => x.ans.DefaultIfEmpty(),
                //                   (x, y) => new QuesWithAnswers
                //                   {
                //                      ques_id = x.ques.ques_id,
                //                      lang_code=x.ques.lang_code,


                //                   }
                //       ).ToList();

                return result.ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {

            try
            {
                if (lang.ToLower() == "all")
                {
                    return await _db.RegistrationQuestions.ToListAsync();
                }
                else
                {
                    return await _db.RegistrationQuestions.Where(wr => wr.lang_code == lang).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region "insert & update"
        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst, string client_id, string FullName, string email)
        {
            int count = 0;
            string? lang = lst.First().lang_code?.ToLower();
            RegsistrationQuesResponse response;
            try
            {
                foreach (RegistrationAnswer answer in lst)
                {
                    answer.client_id = client_id;
                    if (answer.id == 0)
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


                    //generate & save coupon
                    string copounAuto = HelperCls.getCopounText();
                    ClientCopoun copoun = new ClientCopoun { client_id = client_id, copoun = copounAuto, id = 0, start_date = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), end_date = DateOnly.Parse("2025-06-06") };
                    if (saveClientCopoun(copoun).success)
                    {
                        //send mail
                        try
                        {
                            string fileName = "ConfirmMail_" + lang + ".html";
                            string htmlBody = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                            htmlBody = htmlBody.Replace("@user", FullName);
                            htmlBody = htmlBody.Replace("@EXPIRY_DATE", "06/06/2025");
                            string htmlRes = htmlBody.Replace("@copoun", copounAuto);
                            MailData Mail_Data = new MailData { EmailToId = email, EmailToName = FullName, EmailSubject = UtilsCls.GetMailSubjectByLang(lang, 1), EmailBody = htmlRes };
                            _mailSettingDao.SendMail(Mail_Data);
                        }
                        catch (Exception e)
                        {
                            return new RegsistrationQuesResponse { success = false, errors = "error in send mail" };
                        }

                        //end send mail
                    }
                    else
                    {
                        return new RegsistrationQuesResponse { success = false, errors = "error in generate copoun" };

                    }
                    response = new RegsistrationQuesResponse { errors = null, success = true, WelcomeMsg = htmlcontent };
                }
                else
                {
                    response = new RegsistrationQuesResponse { errors = "Error in saving List Check Admin", success = false, WelcomeMsg = null };
                }
            }

            catch (Exception ex)
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
                if (ques.ques_id == 0)
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
                response = new ResponseCls { success = true, errors = null };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message };
            }
            return response;
        }


        public ResponseCls saveClientCopoun(ClientCopoun copoun)
        {
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                if (copoun.id == 0)
                {
                    if (_db.ClientCopouns.Count() > 0)
                    {
                        //check client id validation
                        var result = _db.ClientCopouns.Where(wr => wr.client_id == copoun.client_id && wr.start_date == copoun.start_date && wr.end_date == copoun.end_date).SingleOrDefault();
                        if (result != null)
                        {
                            return new ResponseCls { success = false, errors = "duplicate data" };
                        }

                        maxId = _db.ClientCopouns.Max(d => d.id);

                    }

                    copoun.id = maxId + 1;
                    _db.ClientCopouns.Add(copoun);
                }
                else
                {
                    _db.Update(copoun);
                }

                _db.SaveChanges();
                response = new ResponseCls { success = true, errors = null };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message };
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

        public ResponseCls saveClientServices(List<ClientService> lst, string client_id)
        {
            int count = 0;
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                foreach (ClientService service in lst)
                {
                    service.client_id = client_id;
                    if (service.id == 0)
                    {
                        if (_db.ClientServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.ClientServices.Where(wr => wr.client_id == service.client_id && wr.productId == service.productId).SingleOrDefault();
                            if (result != null)
                            {
                                return new ResponseCls { success = false, errors = "duplicate data" };
                            }

                            maxId = _db.ClientServices.Max(d => d.id);


                        }
                       
                        service.id = maxId + 1;
                        _db.ClientServices.Add(service);
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.ClientServices.Update(service);
                        _db.SaveChanges();
                    }

                    count++;
                }

               
                if (count == lst.Count)
                {
                    response = new ResponseCls { errors = null, success = true };
                }
                else
                {
                    response = new ResponseCls { errors = "Error in saving data Check Admin", success = false };
                }
            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = ex.Message, success = false };
            }

            return response;
        }
        public async Task<List<Product_Tree>> GetProduct_Tree(string clientId)
        {

            try
            {
                var main = await _db.Products.ToListAsync();

                var result = GetProduct_TreeMain(main, 0, clientId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> CheckServiceSelected(int productId, string clientId)
        {
            try
            {
                var count = _db.ClientServices.Where(wr => wr.client_id == clientId && wr.productId == productId).CountAsync();
                if (count.Result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<Product_Tree> GetProduct_TreeMain(List<Product> lst, int parentId, string clientId)
        {

            return lst
                   .Where(x => x.productParent == parentId)
                   .ToList()
                  .Select(s => new Product_Tree
                  {
                      productParent = s.productParent,

                      productId = s.productId,
                      productName = s.productName,
                      product_desc = s.product_desc,
                      children = GetProduct_TreeMain(lst, s.productId, clientId).ToList(),
                      isSelected = CheckServiceSelected(s.productId, clientId).Result
                  })
                .ToList();
        }

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

        public async Task<List<ClientProfileCast>> GetClientProfiles(string clientId)
        {
            try
            {
                return await _db.ClientProfiles.Where(wr => wr.client_id == clientId).Select(slc => new ClientProfileCast
                {
                    client_birthday = slc.client_birthday,
                    client_birthdayStr = DateTime.Parse(slc.client_birthday.ToString()).ToString("yyyy-MM-dd"),
                    client_email = slc.client_email,
                    client_id = slc.client_id,
                    client_name = slc.client_name,
                    fb_link = slc.fb_link,
                    gender = slc.gender,
                    lang = slc.lang,
                    nation = slc.nation,
                    pay_code = slc.pay_code,
                    phone_number = slc.phone_number,
                    profile_id = slc.profile_id,
                    twitter_link = slc.twitter_link
                }).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public ResponseCls saveMainProfile(ClientProfileCast profile)
        {
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                profile.client_birthday = DateOnly.Parse(profile.client_birthdayStr, CultureInfo.InvariantCulture);
                if (profile.profile_id == 0)
                {
                    if (_db.ClientProfiles.Count() > 0)
                    {
                        //check validate
                        if (_db.ClientProfiles.Where(wr => wr.client_id == profile.client_id).Count() == 0)
                        {
                            maxId = _db.ClientProfiles.Max(d => d.profile_id);
                        }
                        else
                        {
                            //do no thing duplicate data
                            return new ResponseCls { success = false, errors = "duplicate data" };

                        }

                    }
                    profile.profile_id = maxId + 1;
                    _db.ClientProfiles.Add(profile);
                }
                else
                {
                    _db.Update(profile);
                }

                _db.SaveChanges();
                response = new ResponseCls { success = true, errors = null, idOut = profile.profile_id };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message, idOut = 0 };
            }
            return response;
        }

        public ResponseCls saveClientBrand(ClientBrand brand)
        {
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                if (brand.id == 0)
                {
                    if (_db.ClientBrands.Count() > 0)
                    {
                        maxId = _db.ClientBrands.Max(d => d.id);

                    }


                    brand.id = maxId + 1;
                    _db.ClientBrands.Add(brand);
                }
                else
                {
                    _db.Update(brand);
                }

                _db.SaveChanges();
                response = new ResponseCls { success = true, errors = null, idOut = brand.id };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message, idOut = 0 };
            }
            return response;
        }

        public async Task<ResponseCls> saveProfileImage(ClientImage image)
        {
            ResponseCls response;
            decimal maxId = 0;
            try
            {

                var result = _db.ClientImages.Where(wr => wr.client_id == image.client_id && wr.type == image.type).SingleOrDefault();
                if (result != null)
                {
                    result.img_path = image.img_path;
                    result.img_name = image.img_name;
                    _db.Update(result);
                }
                else
                {
                    if (_db.ClientImages.Count() > 0)
                    {
                        maxId = await _db.ClientImages.DefaultIfEmpty().MaxAsync(d => d.id);

                    }

                    image.id = maxId + 1;
                    _db.ClientImages.Add(image);
                }


                _db.SaveChanges();
                response = new ResponseCls { success = true, errors = null, idOut = image.id };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { success = false, errors = ex.Message, idOut = 0 };
            }
            return response;
        }

        public async Task<List<ClientImage>> GetProfileImage(string clientId)
        {
            try
            {
                return await _db.ClientImages.Where(wr => wr.client_id == clientId).Select(s => new ClientImage
                {
                    id = s.id,
                    client_id = s.client_id,
                    img_name = s.img_name,
                    type = s.type,
                    img_path = "https://waslaa.de:4431//" + s.img_path
                }).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}

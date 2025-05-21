﻿using Mails_App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
                //tbl RegistrationQuestions => contains question
                //tbl RegistrationAnswers => contain users answers (ques_id,answer.clientId)
                //make left jpon with two tables , to check if user answer this question before or not
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

                return result.ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get Question List only
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
        //save user's answers for registerations questions 
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
                        //check duplicate validation
                        var result = _db.RegistrationAnswers.Where(wr => wr.client_id == answer.client_id && wr.ques_id == answer.ques_id && wr.lang_code == answer.lang_code).SingleOrDefault();
                        if (result != null)
                        {
                            return new RegsistrationQuesResponse { success = false, errors = "duplicate data",WelcomeMsg=null };
                        }
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
                        //send confirmation mail
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
                    response = new RegsistrationQuesResponse { errors = null, success = true, WelcomeMsg = HelperCls.getResponseMsg(lang) };
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

        //save registration questions (used in admin)
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

        //delete registration questions (used in admin)
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
                //check if client saved profile image before or not (save or update)
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
                return await _db.ClientImages.Where(wr => wr.client_id == clientId && wr.type == 1).Select(s => new ClientImage
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


        #region "packages &services"

        //assign  Services to package

        public ResponseCls SavePricingPKgServicesLst(List<PricingPkgService> lst)
        {
            int count = 0;
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                foreach (PricingPkgService row in lst)
                {

                    if (row.id == 0)
                    {
                        if (_db.PricingPkgServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.PricingPkgServices.Where(wr => wr.package_id == row.package_id && wr.service_id == row.service_id).SingleOrDefault();
                            if (result != null)
                            {
                                return new ResponseCls { success = false, errors = "duplicate data" };
                            }

                            maxId = _db.PricingPkgServices.Max(d => d.id);


                        }

                        row.id = maxId + 1;
                        _db.PricingPkgServices.Add(row);
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.PricingPkgServices.Update(row);
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
    
        //public ResponseCls SavePricingPKgServices(PricingPkgService service)
        //{
        //    ResponseCls response;
        //    decimal maxId = 0;
        //    try
        //    {
        //        if (service.id == 0)
        //        {
        //            if (_db.PricingPkgServices.Count() > 0)
        //            {
        //                //check duplicate validation
        //                var result = _db.PricingPkgServices.Where(wr => wr.package_id == service.package_id && wr.service_id == service.service_id).SingleOrDefault();
        //                if (result != null)
        //                {
        //                    return new ResponseCls { success = false, errors = "duplicate data" };
        //                }

        //                maxId = _db.PricingPkgServices.Max(d => d.id);



        //            }
        //            service.id = maxId + 1;
        //            _db.PricingPkgServices.Add(service);
        //            _db.SaveChanges();
        //        }
        //        else
        //        {
        //            _db.PricingPkgServices.Update(service);
        //            _db.SaveChanges();
        //        }

        //        response = new ResponseCls { errors = null, success = true };


        //    }

        //    catch (Exception ex)
        //    {
        //        response = new ResponseCls { errors = ex.Message, success = false };
        //    }
        //    return response;
        //}
        //get packages data by lang and currency
        public async Task<List<PricingPackageCast>> GetPricingPackageWithService(LangReq req)
        {

            try
            {
                var fullEntries = await _db.PricingPkgServices
                    .Join(
                        _db.PricingPackages.Where(wr => wr.lang_code == req.lang),
                        Service => Service.package_id,
                        PKG => PKG.package_id,
                        (Service, PKG) => new { Service, PKG }
                    )
                    .Join(
                        _db.PricingPkgCurrencies.Where(wr => wr.curr_code.ToLower() == req.curr_code.ToLower()),
                        combinedEntry => combinedEntry.PKG.package_id,
                        curr => curr.package_id,
                        (combinedEntry, curr) => new ServicesWithPkg
                        {
                            curr_code = curr.curr_code,
                            package_id = combinedEntry.Service.package_id,
                            discount_amount = curr.discount_amount,
                            discount_type = curr.discount_type,
                            package_desc = combinedEntry.PKG.package_desc,
                            package_details = combinedEntry.PKG.package_details,
                            package_name = combinedEntry.PKG.package_name,
                            package_price = curr.package_price,
                            package_sale_price = curr.package_sale_price,
                            servicepkg_id = combinedEntry.Service.id,
                            service_id = combinedEntry.Service.service_id,
                            service_name = combinedEntry.Service.service_name
                        }
                    ).ToListAsync();


               return fullEntries.GroupBy(grp => new
                 {
                   grp.package_id,
                   grp.package_name,   
                   grp.package_sale_price,
                   grp.package_price,
                   grp.package_desc,
                   grp.package_details,
                   grp.discount_amount,
                   grp.discount_type,
                   grp.curr_code
                 }).Select(s => new PricingPackageCast
                 {
                     package_details=s.Key.package_details,
                     package_desc=s.Key.package_desc,
                     package_price=s.Key.package_price,
                     package_sale_price=s.Key.package_sale_price,
                     package_name=s.Key.package_name,
                     curr_code=s.Key.curr_code,
                     package_id=s.Key.package_id,
                     services = fullEntries.Where(wr => wr.package_id == s.Key.package_id).ToList(),
                 }).ToList();
                //return await _db.PricingPackages.Where(wr => wr.lang_code == req.lang).Select(s => new PricingPackageCast
                //{
                //    lang_code = s.lang_code,
                //    end_date = s.end_date,
                //    end_dateStr =s.end_date.ToString(),
                //    package_desc = s.package_desc,
                //    package_details = s.package_details,
                //    package_id = s.package_id,
                //    package_name = s.package_name,
                //    package_price= s.package_price,
                //    package_sale_price = s.package_sale_price,
                //    start_date = s.start_date,
                //    start_dateStr=s.start_date.ToString(),
                //    active=s.active
                //}).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //save main Package data by admin
        public ResponseCls SavePricingPackage(PricingPackage package)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                //package.start_date = DateTime.Parse(package.start_dateStr);
                //package.end_date = DateTime.Parse(package.end_dateStr);
                if (package.package_id == 0)
                {
                    if (_db.PricingPackages.Count() > 0)
                    {
                        //check duplicate validation
                        var result = _db.PricingPackages.Where(wr => wr.package_name == package.package_name && wr.active == package.active).SingleOrDefault();
                        if (result != null)
                        {
                            return new ResponseCls { success = false, errors = "duplicate data" };
                        }

                        maxId = _db.PricingPackages.Max(d => d.package_id);



                    }
                    package.package_id = maxId + 1;
                    _db.PricingPackages.Add(package);
                    _db.SaveChanges();
                }
                else
                {
                    _db.PricingPackages.Update(package);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = ex.Message, success = false };
            }

            return response;
        }

        //save prices with currency for packages
        public ResponseCls SavePricingPackageCurrency(PricingPkgCurrencyCast cast)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                PricingPkgCurrency currency = new PricingPkgCurrency { 
                    start_date  = DateTime.Parse(cast.start_dateStr) ,
                    active = cast.active,
                    curr_code = cast.curr_code,
                    discount_amount = cast.discount_amount,
                    discount_type = cast.discount_type,
                    end_date=DateTime.Parse(cast.end_dateStr),
                    id = cast.id,
                    package_id = cast.package_id,
                    package_price = cast.package_price,
                    package_sale_price=cast.package_sale_price,
                };
                if (currency.id == 0)
                {
                    if (_db.PricingPkgCurrencies.Count() > 0)
                    {
                        //check duplicate validation
                        var result = _db.PricingPkgCurrencies.Where(wr => wr.package_id == currency.package_id && wr.curr_code.ToLower() == currency.curr_code.ToLower() && wr.start_date == currency.start_date && wr.end_date == currency.end_date ).SingleOrDefault();
                        if (result != null)
                        {
                            return new ResponseCls { success = false, errors = "duplicate data" };
                        }

                        maxId = _db.PricingPkgCurrencies.Max(d => d.id);



                    }
                    currency.id = maxId + 1;
                    _db.PricingPkgCurrencies.Add(currency);
                    _db.SaveChanges();
                }
                else
                {
                    _db.PricingPkgCurrencies.Update(currency);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = ex.Message, success = false };
            }

            return response;
        }

        //get Package's prices with currency
        public async Task<List<PricingPkgCurrencyCast>> GetPricingPkgCurrency(PricingPkgCurrencyReq req)
        {

            try
            {
                return await _db.PricingPkgCurrencies.Where(wr => wr.active == req.active && wr.package_id == req.package_id && wr.curr_code.ToLower() == (req.curr_code.ToLower() == "all" ? wr.curr_code.ToLower() : req.curr_code.ToLower()))
                    .Select(s => new PricingPkgCurrencyCast
                    {
                        package_id=s.package_id,
                        curr_code=s.curr_code,
                        active=s.active,
                        discount_amount=s.discount_amount,
                        discount_type=s.discount_type,
                        end_dateStr=s.end_date.ToString(),
                        end_date=s.end_date,
                        id=s.id,
                        package_price=s.package_price,
                        package_sale_price=s.package_sale_price,
                        start_date=s.start_date,
                        start_dateStr=s.start_date.ToString()

                    })
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get Parent products  
        public async Task<List<Service>> GetProduct(ServiceReq req)
        {

            try
            {
                return await _db.Services.Where(wr => wr.active == req.active && wr.productParent == (req.parent == -1 ? wr.productParent : req.parent)).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //save product (service) data by admin
        public ResponseCls SaveProduct(Service service)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                if (service.productId == 0)
                {
                    if (_db.Services.Count() > 0)
                    {
                        //check duplicate validation
                        var result = _db.Services.Where(wr => wr.productId == service.productId && wr.productParent == service.productParent).SingleOrDefault();
                        if (result != null)
                        {
                            return new ResponseCls { success = false, errors = "duplicate data" };
                        }

                        maxId = _db.Services.Max(d => d.productId);


                    }

                    service.productId = maxId + 1;
                    _db.Services.Add(service);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Services.Update(service);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = ex.Message, success = false };
            }

            return response;
        }


        //save Client's services List
        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id)
        {
            int count = 0;
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                foreach (ClientServiceCast row in lst)
                {

                    ClientService service = new ClientService { client_id = client_id, id = row.id, productId = row.productId,package_id=row.package_id };
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
                        if (row.isSelected == true)
                        {
                            _db.ClientServices.Update(service);
                        }
                        else
                        {
                            _db.ClientServices.Remove(service);
                            _db.SaveChanges();
                        }
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


        // get products as tree (used in admin & website) depend on client Id , 
        //in website get only active products, and for each one check if client selected or not
        //in admin get all (active or not active), and not check againts client slected or not
        public async Task<List<Service_Tree>> GetProduct_Tree(string clientId, string lang)
        {

            try
            {
                var main = new List<Service>();
                if (clientId == "admin")
                {
                    main = await _db.Services.ToListAsync();
                }
                else
                {
                    main = await _db.Services.Where(wr => wr.active == true && wr.lang_code == lang).ToListAsync();
                }


                var result = GetProduct_TreeMain(main, 0, clientId).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<ClientService> CheckServiceSelected(int productId, string clientId)
        {

            try
            {
                var result = await _db.ClientServices.Where(wr => wr.client_id == clientId && wr.productId == productId).SingleOrDefaultAsync();
                if (result == null)
                {
                    return new ClientService();
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                return new ClientService();
            }
        }
        public List<Service_Tree> GetProduct_TreeMain(List<Service> lst, int parentId, string clientId)
        {

            return lst
                   .Where(x => x.productParent == parentId)
                   .ToList()
                  .Select(s => new Service_Tree
                  {
                      leaf= s.leaf,
                      lang_code = s.lang_code,
                      productParent = s.productParent,
                      productId = s.productId,
                      productName = s.productName,
                      product_desc = s.product_desc,
                      active = s.active,
                      children = GetProduct_TreeMain(lst, s.productId, clientId).ToList(),
                      clientServiceId = clientId == "admin" ? 0 : CheckServiceSelected(s.productId, clientId).Result.id,
                      isSelected = clientId == "admin" ? false : (CheckServiceSelected(s.productId, clientId).Result.id > 0 ? true : false)
                  })
                .ToList();
        }
        #endregion "packages &services"

    }
}

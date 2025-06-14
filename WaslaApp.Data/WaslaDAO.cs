using Mails_App;
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
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.invoices;
using WaslaApp.Data.Models.PackagesAndServices;
using WaslaApp.Data.Models.profile;
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
                    ClientCopoun copoun = new ClientCopoun { client_id = client_id,discount_value=50,discount_type=1, copoun = copounAuto, id = 0, start_date = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), end_date = DateOnly.Parse("2026-06-06") };
                    if (saveClientCopoun(copoun).success)
                    {
                        //send confirmation mail
                        try
                        {
                            string fileName = "ConfirmMail_" + lang + ".html";
                            string htmlBody = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                            htmlBody = htmlBody.Replace("@user", FullName);
                            htmlBody = htmlBody.Replace("@EXPIRY_DATE", copoun.end_date.ToString());
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

        //get all invoices by client_id
        public async Task<List<ClientInvoiceGrp>> GetInvoicesByClient(string client_id)
        {
            try
            {
                var fullEntries = await _db.ClientServices.Where(wr => wr.client_id == client_id)
                                .Join(
                                        _db.InvoiceMains.Where(wr => wr.active == true && wr.status ==1),
                                        SERV => new { SERV.invoice_id, SERV.client_id },
                                        INV => new { INV.invoice_id, INV.client_id },
                                        (SERV, INV) => new { SERV, INV }
                                     )
                                .Join(
                                     _db.ApplyTaxes,
                                     SERV_INV => SERV_INV.INV.tax_id,
                                     TAX => TAX.tax_id,
                                     (SERV_INV, TAX) => new { SERV_INV, TAX }
                                    )
                                 .Join(
                                    _db.PricingPackages,
                                    SERV_PKG => SERV_PKG.SERV_INV.SERV.package_id,
                                    PKG => PKG.package_id,
                                    (SERV_PKG, PKG) => new { SERV_PKG, PKG }
                                    )
                                 .Join(
                                    _db.Services,
                                    combinedEntry => combinedEntry.SERV_PKG.SERV_INV.SERV.productId,
                                    PRODUCT => PRODUCT.productId,
                                    (combinedEntry, PRODUCT) => new ClientInvoiceResponse
                                    {
                                        invoice_id = combinedEntry.SERV_PKG.SERV_INV.INV.invoice_id,
                                        curr_code = combinedEntry.SERV_PKG.SERV_INV.INV.curr_code,
                                        discount = combinedEntry.SERV_PKG.SERV_INV.INV.discount,
                                        total_price = combinedEntry.SERV_PKG.SERV_INV.INV.total_price,
                                        grand_total_price = combinedEntry.SERV_PKG.SERV_INV.INV.grand_total_price,
                                        service_id = combinedEntry.SERV_PKG.SERV_INV.SERV.productId,
                                        package_id = combinedEntry.SERV_PKG.SERV_INV.SERV.package_id,
                                        service_name = PRODUCT.productName,
                                        package_name = combinedEntry.PKG.package_name,
                                        package_price = combinedEntry.PKG.package_price,
                                        package_sale_price = combinedEntry.PKG.package_sale_price,  
                                        package_desc = combinedEntry.PKG.package_desc,
                                        package_details = combinedEntry.PKG.package_details,
                                        invoice_code = combinedEntry.SERV_PKG.SERV_INV.INV.invoice_code,
                                        invoice_code_auto = combinedEntry.SERV_PKG.SERV_INV.INV.invoice_code_auto,
                                        status = combinedEntry.SERV_PKG.SERV_INV.INV.status,
                                        tax_amount= combinedEntry.SERV_PKG.TAX.tax_amount,
                                        tax_code= combinedEntry.SERV_PKG.TAX.tax_code

                                    }
                                   ).ToListAsync();
                var result = fullEntries.GroupBy(grp => new
                {
                    grp.invoice_id,
                    grp.curr_code,
                    grp.discount,
                    grp.grand_total_price,
                    grp.invoice_code,
                    grp.invoice_code_auto,
                    grp.status,
                    grp.total_price,
                    grp.tax_code,
                    grp.tax_amount
                }).Select(s => new ClientInvoiceGrp
                {
                    invoice_id = s.Key.invoice_id,
                    invoice_code_auto = s.Key.invoice_code_auto,
                    status = s.Key.status,
                    invoice_code = s.Key.invoice_code,
                    curr_code = s.Key.curr_code,
                    discount = s.Key.discount,
                    grand_total_price = s.Key.grand_total_price,
                    total_price = s.Key.total_price,
                    tax_amount=s.Key.tax_amount,
                    tax_code=s.Key.tax_code,
                    pkgs = fullEntries.Where(wr => wr.invoice_id == s.Key.invoice_id).ToList()
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ResponseCls RemoveInvoice(InvRemoveReq req, string client_id)
        {
            try
            {
                ClientService service = _db.ClientServices.Where(wr => wr.invoice_id == req.invoice_id && wr.package_id == req.package_id && wr.productId == req.service_id).SingleOrDefault();
                if(service != null)
                {
                    _db.Remove(service);
                    _db.SaveChanges();
                    int count = _db.ClientServices.Where(wr => wr.invoice_id == req.invoice_id).Count();
                    if (count == 0)
                    {
                        InvoiceMain inv = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.invoice_id == req.invoice_id).SingleOrDefault();
                        if (inv != null)
                        {
                            inv.active = false;
                            _db.Update(inv);
                            _db.SaveChanges();

                        }
                    }
                    return new ResponseCls { success = true, errors = null };
                }
                
               
                return new ResponseCls { success = false, errors = "no Invoice Founded" };
            }
            catch (Exception ex)
            {
                return new ResponseCls { success = false, errors = ex.Message };
            }

        }
        public ResponseCls CheckoutInvoice(CheckoutReq req, string client_id)
        {
            try
            {
                InvoiceMain inv = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.invoice_id == req.invoice_id).SingleOrDefault();
                if (inv != null)
                {
                    inv.status = 2;
                    inv.copoun_id = req.copoun_id;
                    inv.grand_total_price = inv.grand_total_price - (inv.grand_total_price * req.copoun_discount / 100);
                    _db.Update(inv);
                    _db.SaveChanges();
                    return new ResponseCls { success = true, errors = null };
                }
                return new ResponseCls { success = false, errors = "no Invoice Founded" };
            }
            catch (Exception ex)
            {
                return new ResponseCls { success = false, errors = ex.Message };
            }

        }
        //validate client discount coupon
        public async Task<List<ClientCopounCast>> ValidateClientCopoun(ClientCopounReq req,string client_id)
        {

            try
            {
                return await _db.ClientCopouns.Where(wr => wr.client_id == client_id && wr.copoun == req.copoun)
                                              .Select(s => new ClientCopounCast
                                              {
                                                  client_id = s.client_id,
                                                  copoun=req.copoun,
                                                  discount_type=s.discount_type,
                                                  discount_value=s.discount_value,
                                                  end_date=s.end_date,
                                                  end_dateStr=s.end_date.ToString(),
                                                  id=s.id,
                                                  start_date=s.start_date,
                                                  start_dateStr=s.start_date.ToString(),
                                                  valid=true
                                              }
                                                     )
                                              .ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //get payment methods list
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

        //get brands of specific client
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

        //get profile for the client
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
                    twitter_link = slc.twitter_link,
                    address=slc.address
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
        
        public async Task<decimal?> CalculatePriceWithTax(int taxId, decimal? price)
        {
            decimal? total = 0;
            try
            {
                var taxApply = await _db.ApplyTaxes.Where(wr => wr.tax_id == taxId).SingleOrDefaultAsync();
                if (taxApply != null)
                {
                    if (taxApply.tax_sign.Equals("+"))
                    {
                        total = price + (price * taxApply.tax_amount);
                    }
                    else
                    {
                        total = price - (price * taxApply.tax_amount);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return total;
        }
        public InvoiceResponse MakeClientInvoiceForPackages(List<InvoiceReq> lst, string client_id ,string client_name,string client_email)
        {
            InvoiceResponse response;
            int count = 0;
            decimal maxId = 0;
            decimal servicemaxId = 0;
            //concatenate each package code together + date now to make unique and readable code
            string invCode = string.Join("-", lst.Select(e => e.package_code == null ? "00" : e.package_code)) + "-" + DateTime.Now.ToString("yyyyMMdd");

            //first save in InvoiceMain (make invoice)
            var totalPrice = lst.Sum(s => s.package_sale_price);
            //var totalDiscount = lst.Sum(s => s.discount_amount);
            var TotalPriceAfterTax = CalculatePriceWithTax(1, totalPrice).Result;
            InvoiceMain main = new InvoiceMain
            {
                invoice_id = 0,
                client_id = client_id,
                client_email = client_email,
                active = true,
                client_name = client_name,
                invoice_date = DateTime.Now,
                curr_code = lst.FirstOrDefault().curr_code,
                discount = 0,
                total_price = totalPrice,
                invoice_code = invCode,
                copoun_id=0,
                grand_total_price = TotalPriceAfterTax,
                status=1, //mean pending
                tax_id=1

            };
            try
            {
              
                if (_db.InvoiceMains.Count() > 0)
                {
                    //check duplicate validation
                    var result = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.active == main.active && wr.invoice_code == main.invoice_code && wr.invoice_date == main.invoice_date).SingleOrDefault();
                    if (result != null)
                    {
                        return new InvoiceResponse { success = false, errors = "duplicate data" };
                    }

                    maxId = _db.InvoiceMains.Max(d => d.invoice_id);



                }
                main.invoice_id = maxId + 1;
                main.invoice_code_auto = "INV" + main.invoice_id;
                _db.InvoiceMains.Add(main);
                _db.SaveChanges();

                
                response = new InvoiceResponse { success = true, idOut = main.invoice_id };
            }
            catch (Exception ex) {
                response = new InvoiceResponse { errors = ex.Message, success = false, idOut = 0 };
            }
            // if invoice main sucess continue to save packages details
            if (response.success) {
                //second save Client services List
                foreach (InvoiceReq row in lst)
                {

                    ClientService service = new ClientService { client_id = client_id, id = 0, productId = row.productId, package_id = row.package_id,invoice_id = response.idOut };
                        if (_db.ClientServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.ClientServices.Where(wr => wr.client_id == service.client_id && wr.productId == service.productId && wr.package_id == service.package_id && wr.invoice_id == service.invoice_id).SingleOrDefault();
                            if (result != null)
                            {
                                return new InvoiceResponse { success = false, errors = "duplicate data" };
                            }

                        servicemaxId = _db.ClientServices.Max(d => d.id);


                        }
                        service.id = servicemaxId + 1;
                        _db.ClientServices.Add(service);
                        _db.SaveChanges();
                   
                    count++;
                }


                if (count == lst.Count)
                {
                    //get client profile data

                    ClientProfileCast profile =  GetClientProfiles(client_id).Result.SingleOrDefault();
                    
                    //start send invoice mail

                    response = new InvoiceResponse
                    { 
                        errors = null, 
                        success = true ,
                        invoice = new HtmlInvoice
                        {
                            address= profile !=null? profile.address : "",
                            contact= profile != null ?  profile.phone_number : "",
                            Discount =main.discount,
                            services=lst,
                            InvoiceNo= main.invoice_code,
                            IssuedDate=main.invoice_date?.ToString("yyyy-MM-dd"),
                            Total=main.total_price,
                            SubTtotal = main.total_price,
                            user=client_name

                        }

                    };
                }
                else
                {
                    response = new InvoiceResponse { errors = "Error in saving data Check Admin", success = false };
                }
            }
            return response;
        }
       
        
       
        public async Task<List<PricingPackageWithService>> GetPricingPackages(PricingPackageReq req)
        {

            try
            {
               return await _db.PricingPackages.Where(wr => wr.active == req.active &&
                                                        wr.service_id == (req.service_id == 0 ? wr.service_id : req.service_id) &&
                                                        wr.lang_code == (req.lang_code.ToLower() == "all" ? wr.lang_code : req.lang_code) &&
                                                        wr.curr_code == (req.curr_code.ToLower() == "all" ? wr.curr_code : req.curr_code) &&
                                                        wr.active == req.active)
                   .Join(
                       _db.Services,
                        PKG => new { PKG.service_id , PKG.lang_code},
                        Service => new { service_id = Service.productId, Service.lang_code },
                       (PKG, Service) => new PricingPackageWithService
                       {
                           service_id = PKG.service_id,
                           curr_code = PKG.curr_code,
                           lang_code = PKG.lang_code,
                           package_sale_price = PKG.package_sale_price,
                           active = PKG.active,
                           discount_amount = PKG.discount_amount,
                           discount_type = PKG.discount_type,
                           package_price = PKG.package_price,
                           package_name = PKG.package_name,
                           package_id = PKG.package_id,
                           package_desc = PKG.package_desc,
                           start_date = PKG.start_date,
                           end_date = PKG.end_date,
                           order = PKG.order,
                           package_details = PKG.package_details,
                           service_name = Service.productName,
                           service_code=Service.service_code,
                           is_recommend=PKG.is_recommend,
                           package_code=PKG.package_code
                         
                       }
                   ).ToListAsync();
               

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<PricingPkgFeature> GetPricingPkgFeatures(PricingPkgFeatureReq req)
        {

            try
            {
                return  _db.PricingPkgFeatures.Where(wr => wr.active == req.active && wr.package_id == req.package_id && wr.lang_code == req.lang_code).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //assign  features to package 
        public ResponseCls SavePricingPKgFeatureLst(List<PricingPkgFeature> lst)
        {
            int count = 0;
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                foreach (PricingPkgFeature row in lst)
                {

                    if (row.id == 0)
                    {
                        if (_db.PricingPkgFeatures.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.PricingPkgFeatures.Where(wr => wr.package_id == row.package_id && wr.feature_name == row.feature_name && wr.active == row.active).SingleOrDefault();
                            if (result != null)
                            {
                                return new ResponseCls { success = false, errors = "duplicate data" };
                            }

                            maxId = _db.PricingPkgFeatures.Max(d => d.id);


                        }

                        row.id = maxId + 1;
                        _db.PricingPkgFeatures.Add(row);
                        _db.SaveChanges();
                    }
                    else
                    {
                        _db.PricingPkgFeatures.Update(row);
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

       
        //get packages data by lang and currency

        public async Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req)
        {
            try
            {
            
                var fullEntries = await _db.PricingPackages.Where(wr => wr.lang_code == req.lang &&  wr.curr_code.ToLower() == req.curr_code.ToLower() && wr.active == true)
                    .Join(
                        _db.Services.Where(wr => wr.lang_code == req.lang),
                         PKG => new { PKG.service_id },
                         Service => new { service_id = Service.productId },
                        (PKG, Service) => new PricingPackageCast
                        {
                            service_id= PKG.service_id,
                            curr_code= PKG.curr_code,
                            lang_code= PKG.lang_code,
                            package_sale_price= PKG.package_sale_price,
                            active= PKG.active,
                            discount_amount = PKG.discount_amount,
                            discount_type= PKG.discount_type,
                            end_dateStr= PKG.start_date.ToString(),
                            package_price= PKG.package_price,
                            package_name= PKG.package_name,
                            package_id= PKG.package_id,
                            package_desc= PKG.package_desc,
                            start_date=PKG.start_date,
                            end_date = PKG.end_date,
                            order= PKG.order,
                            package_details= PKG.package_details,
                            service_name= Service.productName,
                            start_dateStr=PKG.start_date.ToString(),
                            is_recommend = PKG.is_recommend,
                            package_code = PKG.package_code,
                            isSelected= false,
                            service_code = Service.service_code,
                            //features=  GetPricingPkgFeatures(new PricingPkgFeatureReq { active=true,lang_code=req.lang,package_id= PKG.package_id }).ToList()
                        }
                    ).ToListAsync();
                fullEntries = fullEntries.Select(s => new PricingPackageCast
                {
                    service_id = s.service_id,
                    curr_code = s.curr_code,
                    lang_code = s.lang_code,
                    package_sale_price = s.package_sale_price,
                    active = s.active,
                    discount_amount = s.discount_amount,
                    discount_type = s.discount_type,
                    end_dateStr = s.start_date.ToString(),
                    package_price = s.package_price,
                    package_name = s.package_name,
                    package_id = s.package_id,
                    package_desc = s.package_desc,
                    start_date = s.start_date,
                    end_date = s.end_date,
                    order = s.order,
                    package_details = s.package_details,
                    service_name = s.service_name,
                    start_dateStr = s.start_date.ToString(),
                    is_recommend = s.is_recommend,
                    package_code = s.package_code,
                    isSelected=false,
                   // isSelected = req.client_id == null ? false : (CheckServiceSelected(s.service_id, s.package_id, req.client_id).Result.id > 0 ? true : false),
                    features =  GetPricingPkgFeatures(new PricingPkgFeatureReq { active=true,lang_code=req.lang,package_id= s.package_id }).ToList()
                }).ToList();
                return fullEntries.GroupBy(grp => new
                {
                    grp.service_id,
                    grp.service_name,
                }).Select(s => new ServicesWithPkg
                {
                    service_id = s.Key.service_id,
                    service_name = s.Key.service_name,
                    pkgs = fullEntries.Where(wr => wr.service_id == s.Key.service_id).ToList()
                }).ToList();
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
                //PricingPackage package = new PricingPackage
                //{
                //    start_date = DateTime.Parse(row.start_dateStr),
                //    end_date = DateTime.Parse(row.end_dateStr),
                //    order= row.order,
                //    service_id = row.service_id,
                //    active=row.active,
                //    curr_code= row.curr_code,
                //    discount_amount= row.discount_amount,
                //    discount_type= row.discount_type,
                //    lang_code=row.lang_code,
                //    package_desc= row.package_desc,
                //    package_details= row.package_details,
                //    package_id= row.package_id,
                //    package_name= row.package_name,
                //    package_price= row.package_price,
                //    package_sale_price=row.package_sale_price
                //};

               
                if (package.package_id == 0)
                {
                    if (_db.PricingPackages.Count() > 0)
                    {
                        //check duplicate validation
                        var result = _db.PricingPackages.Where(wr => wr.package_name == package.package_name && wr.active == package.active && wr.lang_code == package.lang_code && wr.curr_code.ToLower() == package.curr_code.ToLower() && wr.service_id == package.service_id).SingleOrDefault();
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

                response = new ResponseCls { errors = null, success = true,idOut= package.package_id };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = ex.Message, success = false,idOut=0 };
            }

            return response;
        }

        //save prices with currency for packages 
        //public ResponseCls SavePricingPackageCurrency(PricingPkgCurrencyCast cast)
        //{
        //    ResponseCls response;
        //    int maxId = 0;
        //    try
        //    {
        //        PricingPkgCurrency currency = new PricingPkgCurrency { 
        //            start_date  = DateTime.Parse(cast.start_dateStr) ,
        //            active = cast.active,
        //            curr_code = cast.curr_code,
        //            discount_amount = cast.discount_amount,
        //            discount_type = cast.discount_type,
        //            end_date=DateTime.Parse(cast.end_dateStr),
        //            id = cast.id,
        //            package_id = cast.package_id,
        //            package_price = cast.package_price,
        //            package_sale_price=cast.package_sale_price,
        //        };
        //        if (currency.id == 0)
        //        {
        //            if (_db.PricingPkgCurrencies.Count() > 0)
        //            {
        //                //check duplicate validation
        //                var result = _db.PricingPkgCurrencies.Where(wr => wr.package_id == currency.package_id && wr.curr_code.ToLower() == currency.curr_code.ToLower() && wr.start_date == currency.start_date && wr.end_date == currency.end_date ).SingleOrDefault();
        //                if (result != null)
        //                {
        //                    return new ResponseCls { success = false, errors = "duplicate data" };
        //                }

        //                maxId = _db.PricingPkgCurrencies.Max(d => d.id);



        //            }
        //            currency.id = maxId + 1;
        //            _db.PricingPkgCurrencies.Add(currency);
        //            _db.SaveChanges();
        //        }
        //        else
        //        {
        //            _db.PricingPkgCurrencies.Update(currency);
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

        //get Package's prices with currency (not used)
        //public async Task<List<PricingPkgCurrencyCast>> GetPricingPkgCurrency(PricingPkgCurrencyReq req)
        //{

        //    try
        //    {
        //        return await _db.PricingPkgCurrencies.Where(wr => wr.active == req.active && wr.package_id == req.package_id && wr.curr_code.ToLower() == (req.curr_code.ToLower() == "all" ? wr.curr_code.ToLower() : req.curr_code.ToLower()))
        //            .Select(s => new PricingPkgCurrencyCast
        //            {
        //                package_id=s.package_id,
        //                curr_code=s.curr_code,
        //                active=s.active,
        //                discount_amount=s.discount_amount,
        //                discount_type=s.discount_type,
        //                end_dateStr=s.end_date.ToString(),
        //                end_date=s.end_date,
        //                id=s.id,
        //                package_price=s.package_price,
        //                package_sale_price=s.package_sale_price,
        //                start_date=s.start_date,
        //                start_dateStr=s.start_date.ToString()

        //            })
        //            .ToListAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

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
                        var result = _db.Services.Where(wr => wr.productId == service.productId && wr.productParent == service.productParent && wr.active == service.active).SingleOrDefault();
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


        //save Client's services List (used to save services's packages selected by user)
        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id)
        {
            int count = 0;
            ResponseCls response;
            decimal maxId = 0;
            try
            {
                foreach (ClientServiceCast row in lst)
                {

                    ClientService service = new ClientService { client_id = client_id, id = row.id, productId = row.productId,package_id=row.package_id,invoice_id=0 };
                    if (service.id == 0)
                    {
                        if (_db.ClientServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.ClientServices.Where(wr => wr.client_id == service.client_id && wr.productId == service.productId && wr.package_id == service.package_id).SingleOrDefault();
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


        public async Task<ClientService> CheckServiceSelected(int productId, int package_id, string clientId)
        {

            try
            {
                var result = await _db.ClientServices.Where(wr => wr.client_id == clientId && wr.package_id == (package_id == 0 ? wr.package_id : package_id) &&  wr.productId == productId).SingleOrDefaultAsync();
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
                      service_code=s.service_code,
                      price=s.price,
                      children = GetProduct_TreeMain(lst, s.productId, clientId).ToList(),
                      clientServiceId = clientId == "admin" ? 0 : CheckServiceSelected(s.productId,0, clientId).Result.id,
                      isSelected = clientId == "admin" ? false : (CheckServiceSelected(s.productId, 0,clientId).Result.id > 0 ? true : false)
                  })
                .ToList();
        }
       
        
        
        #endregion "packages &services"

    }
}

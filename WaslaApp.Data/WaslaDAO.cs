
using Mails_App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Tls;
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
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.invoices;
using WaslaApp.Data.Models.PackagesAndServices;
using WaslaApp.Data.Models.profile;
using static System.Net.Mime.MediaTypeNames;

namespace WaslaApp.Data
{
    public class WaslaDAO
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly MailSettingDao _mailSettingDao;
        private readonly wasla_client_dbContext _db;
        
        public WaslaDAO(wasla_client_dbContext db, MailSettingDao mailSettingDao, IStringLocalizer<Messages> localizer)
        {
            _db = db;
            _mailSettingDao = mailSettingDao;
            _localizer = localizer;
        }

        #region "questions"

        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang)
        {
            try
            {
                //tbl RegistrationQuestions => contains question
                //tbl RegistrationAnswers => contain users answers (ques_id,answer.clientId)
                //make left jpon with two tables , to check if user answer this question before or not
                //var result = from ques in _db.RegistrationQuestions.Where(wr => wr.lang_code == lang)
                //             join ans in _db.RegistrationAnswers.Where(wr => wr.client_id == clientId) on ques.ques_id equals ans.ques_id into quesWans
                //             from m in quesWans.DefaultIfEmpty()
                //             select new QuesWithAnswers
                //             {
                //                 ques_id = ques.ques_id,
                //                 lang_code = ques.lang_code,
                //                 client_id = m.client_id,
                //                 answer = m.answer,
                //                 order = ques.order,
                //                 ques_title = ques.ques_title,
                //                 ques_type = ques.ques_type

                //             };
                var result = from ques in _db.registrationqueswithlangs.Where(wr => wr.lang_code == lang)
                             join ans in _db.RegistrationAnswers.Where(wr => wr.client_id == clientId) on ques.ques_id equals ans.ques_id into quesWans
                             from m in quesWans.DefaultIfEmpty()
                             select new QuesWithAnswers
                             {
                                 ques_id = (int)ques.ques_id,
                                 lang_code = ques.lang_code,
                                 client_id = m.client_id,
                                 answer = m.answer,
                                 order = ques.order,
                                 ques_title = ques.ques_title,
                                // ques_type = ques.ques_type

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

                //if (lang.ToLower() == "all")
                //{
                //    return await _db.RegistrationQuestions.ToListAsync();
                //}
                //else
                //{
                //    return await _db.RegistrationQuestions.Where(wr => wr.lang_code == lang).ToListAsync();
                //}
                return await _db.registrationqueswithlangs.Where(wr => wr.lang_code == lang).Select(s => new RegistrationQuestion
                {
                    order=s.order,
                    lang_code=s.lang_code,
                    ques_id= (int)s.ques_id,
                    ques_title=s.ques_title,
                    ques_type=s.ques_title
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //save user's answers for registerations questions 
        //and send to his mail a coupon code used as offer 
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
                            return new RegsistrationQuesResponse { success = false, errors = _localizer["DuplicateData"], WelcomeMsg = null };
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
                    ClientCopoun copoun = new ClientCopoun { client_id = client_id, discount_value = 50, discount_type = 1, copoun = copounAuto, id = 0, start_date = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), end_date = DateOnly.Parse("2026-06-06") };

                    var Copounresult = saveClientCopoun(copoun);
                    if (Copounresult.success)
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
                            return new RegsistrationQuesResponse { success = false, errors = _localizer["SendMailError"] };
                        }

                        //end send mail
                    }
                    else
                    {
                        return new RegsistrationQuesResponse { success = false, errors = Copounresult.errors };
                        //return new RegsistrationQuesResponse { success = false, errors = _localizer["CopounError"] };

                    }
                    response = new RegsistrationQuesResponse { errors = null, success = true, WelcomeMsg = HelperCls.getResponseMsg(lang) };
                }
                else
                {
                    response = new RegsistrationQuesResponse { errors = _localizer["CheckAdmin"], success = false, WelcomeMsg = null };
                }
            }

            catch (Exception ex)
            {
                response = new RegsistrationQuesResponse { errors = _localizer["CheckAdmin"], success = false, WelcomeMsg = null };
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
                            return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
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
                response = new ResponseCls { success = false, errors = _localizer["CheckAdmin"] };
            }
            return response;
        }
        #endregion
        #region "Profile"

        //update Invoice;s total & grand total price after remove package or  apply Copoun
        public ResponseCls UpdateInvoicePrices(InvUpdatePriceReq req , string client_id)
        {
            try
            {
                InvoiceMain inv = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.invoice_id == req.invoice_id).SingleOrDefault();
                if (inv != null)
                {
                    var totalPrice = req.total_price - req.deduct_amount;
                    var TotalPriceAfterTax = CalculatePriceWithTax(req.tax_id, totalPrice).Result;
                    var totalAfterCopoun = TotalPriceAfterTax - (TotalPriceAfterTax * req.copoun_discount / 100); ;
                    inv.copoun_id = req.copoun_id;
                    inv.total_price = totalPrice;
                    inv.grand_total_price = totalAfterCopoun;
                    _db.Update(inv);
                    _db.SaveChanges();
                    return new ResponseCls { success = true, errors = null };
                }
                return new ResponseCls { success = false, errors = _localizer["NoInvoice"] };
            }
            catch (Exception ex)
            {
                return new ResponseCls { success = false, errors = _localizer["CheckAdmin"] };
            }
        }

        //get all invoices by client_id
        //status =2 mean get invoices which checkout bu user
        // status =1 mean  invoices not checkout by user 

        public async Task<List<ClientInvoiceGrp>> GetInvoicesByClient(ClientInvoiceReq req, string client_id)
        {
            try
            {

                var fullEntries = await _db.clientinvoiceswithdetails.Where(wr => wr.client_id == client_id && wr.active == req.active && (wr.status == (req.status == -1 ? 2 : req.status) || wr.status == (req.status == -1 ? 3 : req.status)))
                                    .Join(
                                   _db.packagesdetailswithservices.Where(wr => wr.lang_code == req.lang_code),
                                   INV => new {
                                       INV.package_id,
                                       service_id = INV.productId,
                                       INV.curr_code,
                                       INV.service_package_id
                                   },
                                   PKG => new { PKG.package_id, PKG.service_id, PKG.curr_code, PKG.service_package_id },
                                   (combinedEntry, PKG) => new ClientInvoiceResponse
                                   {
                                       invoice_id = combinedEntry.invoice_id,
                                       curr_code = combinedEntry.curr_code,
                                       discount = combinedEntry.discount,
                                       total_price = combinedEntry.total_price,
                                       grand_total_price = combinedEntry.grand_total_price,
                                       service_id = combinedEntry.productId,
                                       package_id = combinedEntry.package_id,
                                       service_name = PKG.service_name,
                                       package_name = PKG.package_name,
                                       package_price = PKG.package_price,
                                       package_sale_price = PKG.package_sale_price,
                                       package_desc = PKG.package_desc,
                                       package_details = PKG.package_details,
                                       invoice_code = combinedEntry.invoice_code,
                                       invoice_code_auto = combinedEntry.invoice_code_auto,
                                       status = combinedEntry.status,
                                       tax_amount = combinedEntry.tax_amount,
                                       tax_code = combinedEntry.tax_code,
                                       tax_id = combinedEntry.tax_id,
                                       service_package_id = PKG.service_package_id,
                                       client_name = combinedEntry.client_name,
                                       client_email = combinedEntry.client_email,
                                       invoice_date = DateTime.Parse(combinedEntry.invoice_date.ToString()).ToString("yyyy-MM-dd"),
                                       copoun_id = combinedEntry.copoun_id,
                                       copoun = combinedEntry.copoun,
                                       copoun_discount = combinedEntry.copoun_discount_value,
                                       //invoice_date = combinedEntry.SERV_INV.INV.invoice_date,
                                       // features = GetPricingPkgFeatures(new PricingPkgFeatureReq { active = true, lang_code = req.lang_code, package_id = combinedEntry.SERV_PKG.SERV_INV.SERV.package_id }).ToList()

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
                    grp.tax_amount,
                    grp.tax_id,
                    grp.invoice_date,
                    grp.client_email,
                    grp.client_name,
                    grp.copoun_id,
                    grp.copoun_discount,
                    grp.copoun
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
                    tax_amount = s.Key.tax_amount,
                    tax_code = s.Key.tax_code,
                    tax_id = s.Key.tax_id,
                    client_email=s.Key.client_email,
                    client_name=s.Key.client_name,
                    invoice_date=s.Key.invoice_date,
                    copoun_id=s.Key.copoun_id,
                    copoun=s.Key.copoun,
                    copoun_discount=s.Key.copoun_discount,
                    pkgs = req.status == 2 ? (fullEntries.Where(wr => wr.invoice_id == s.Key.invoice_id)
                                      .Select(s => new ClientInvoiceResponse
                                      {
                                          invoice_id = s.invoice_id,
                                          curr_code = s.curr_code,
                                          discount = s.discount,
                                          total_price = s.total_price,
                                          grand_total_price = s.grand_total_price,
                                          service_id = s.service_id,
                                          package_id = s.package_id,
                                          service_name = s.service_name,
                                          package_name = s.package_name,
                                          package_price = s.package_price,
                                          package_sale_price = s.package_sale_price,
                                          package_desc = s.package_desc,
                                          package_details = s.package_details,
                                          invoice_code = s.invoice_code,
                                          invoice_code_auto = s.invoice_code_auto,
                                          status = s.status,
                                          tax_amount = s.tax_amount,
                                          tax_code = s.tax_code,
                                          tax_id = s.tax_id,
                                          features = getPackageFeatures(new PackageFeatureReq { service_package_id= s.service_package_id,lang_code=req.lang_code }).ToList()
                                      }).ToList()) : fullEntries.Where(wr => wr.invoice_id == s.Key.invoice_id).ToList()

                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        //used to delete client's invoice 
        public ResponseCls RemoveInvoice(InvRemoveReq req, string client_id)
        {
            try
            {
                ClientService service = _db.ClientServices.Where(wr => wr.invoice_id == req.invoice_id && wr.package_id == req.package_id && wr.productId == req.service_id && wr.active == true).SingleOrDefault();
                if(service != null)
                {
                   // _db.Remove(service);
                    service.active = false;
                    _db.Update(service);
                    _db.SaveChanges();
                    int count = _db.ClientServices.Where(wr => wr.invoice_id == req.invoice_id && wr.active == true).Count();
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
                
               
                return new ResponseCls { success = false, errors = _localizer["NoInvoice"] };
            }
            catch (Exception ex)
            {
                return new ResponseCls { success = false, errors = _localizer["CheckAdmin"] };
            }

        }
      
        //make checkout to invoice & send notification mail to customer care
       public ResponseCls CheckoutInvoice(CheckoutReq req, string client_id,string client_name , string completeprofile)
        {
            try
            {
                if(completeprofile.Trim() != "2")
                {
                    return new ResponseCls { success = false, errors = _localizer["ProfileUncomplete"] };
                }    
                InvoiceMain inv = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.invoice_id == req.invoice_id).SingleOrDefault();
                if (inv != null)
                {
                    inv.status = 2;
                    _db.Update(inv);
                    _db.SaveChanges();
                    //send mail to customer care to notify him
                    string fileName = "CustomerNotify.html";
                    string htmlBody = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                    htmlBody = htmlBody.Replace("@user", client_name);
                    htmlBody = htmlBody.Replace("@invoiceNo", req.invoice_code);
                    MailData Mail_Data = new MailData { EmailToId = "Customer.Care@waslaa.de", EmailToName = "Customer.Care@waslaa.de", EmailSubject = UtilsCls.GetMailSubjectByLang("en", 5), EmailBody = htmlBody };
                    _mailSettingDao.SendMail(Mail_Data);
                    return new ResponseCls { success = true, errors = null };
                }
                return new ResponseCls { success = false, errors = _localizer["NoInvoice"] };
            }
            catch (Exception ex)
            {
                return new ResponseCls { success = false, errors = _localizer["CheckAdmin"] };
            }

        }
        //validate client discount coupon
        public async Task<ClientCopounCast> ValidateClientCopoun(ClientCopounReq req,string client_id)
        {
            var msg = _localizer["NoCopoun"];
            try
            {
                var nowDate = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                var result = await _db.ClientCopouns.Where(wr => wr.client_id == client_id && wr.copoun == req.copoun).SingleOrDefaultAsync();
                if (result != null)
                {
                    if (nowDate >= result.start_date && nowDate <= result.end_date)
                    {
                        return new ClientCopounCast
                        {
                            client_id = result.client_id,
                            copoun = req.copoun,
                            discount_type = result.discount_type,
                            discount_value = result.discount_value,
                            end_date = result.end_date,
                            end_dateStr = result.end_date.ToString(),
                            id = result.id,
                            start_date = result.start_date,
                            start_dateStr = result.start_date.ToString(),
                            valid = true,
                            msg = _localizer["CopounValid"]
                        };
                    }
                    else
                    {
                        return new ClientCopounCast
                        {
                           
                            valid = false,
                            msg = _localizer["CopounExpired"]
                        };
                    }

                }
                else
                {
                    return new ClientCopounCast
                    {

                        valid = false,
                        msg = _localizer["NoCopoun"]
                    };
                }

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
                if(profile.client_birthdayStr != null)
                {
                    profile.client_birthday = DateOnly.Parse(profile.client_birthdayStr, CultureInfo.InvariantCulture);
                }
               
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
                            return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };

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
                response = new ResponseCls { success = false, errors = _localizer["CheckAdmin"], idOut = 0 };
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
                response = new ResponseCls { success = false, errors = _localizer["CheckAdmin"], idOut = 0 };
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
                response = new ResponseCls { success = false, errors = _localizer["CheckAdmin"], idOut = 0 };
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
                    img_path = "https://api.waslaa.de//" + s.img_path
                }).ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion


        #region "packages &services"


        //search for Packages' or service's  (name, code, default name)

        public async Task<List<ServicesWithPkg>> GetSearchResult(SearchCls req)
        {
                try
                {
                    var result = await _db.packagesdetailswithservices
                                       .Where(wr => wr.lang_code == req.lang && 
                                              wr.curr_code.ToLower() == (wr.curr_code == null ? wr.curr_code : req.curr_code.ToLower()) &&
                                              wr.active == true &&
                                              (wr.service_name.ToLower().Contains(req.searchTerm.ToLower()) || wr.package_name.ToLower().Contains(req.searchTerm.ToLower()) || wr.service_code.ToLower().Contains(req.searchTerm.ToLower()) || wr.package_code.ToLower().Contains(req.searchTerm.ToLower()))
                                             )
                                       .ToListAsync();
                    if(result != null)
                    {
                        var fullEntries = result.Select(s => new PricingPackageCast
                        {
                            service_id = (int)s.service_id,
                            curr_code = s.curr_code,
                            lang_code = s.lang_code,
                            package_sale_price = s.package_sale_price,
                            active = s.active,
                            discount_amount = s.discount_amount,
                            discount_type = (short?)s.discount_type,
                            package_price = s.package_price,
                            package_name = s.package_name,
                            package_id = (int)s.package_id,
                            package_desc = s.package_desc,
                            order = s.order,
                            package_details = s.package_details,
                            service_name = s.service_name,
                            is_recommend = s.is_recommend,
                            package_code = s.package_code,
                            isSelected = false,
                            is_custom = s.is_custom,
                            service_package_id = s.service_package_id,
                            service_code = s.service_code,
                            features = getPackageFeatures(new PackageFeatureReq { lang_code = req.lang, service_package_id = s.service_package_id }).ToList()

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
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
        }
        public async Task<decimal?> CalculatePriceWithTax(int taxId, decimal? price)
        {
            decimal? total = 0;
            try
            {
                var taxApply = await _db.ApplyTaxes.Where(wr => wr.tax_id == taxId).SingleOrDefaultAsync();
                if (taxApply != null)
                {
                    if (taxApply.tax_sign.Equals('+'))
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
      
        //make Client's Invoice with selected packages 

        public InvoiceResponse MakeClientInvoiceForPackages(List<InvoiceReq> lst, string client_id ,string client_name,string client_email)
        {
            InvoiceResponse response = null;
            //list with custom package => no make invoice directly send contact mail to client
            var customLst = lst.Where(wr => wr.is_custom == true).ToList();
            if(customLst != null && customLst.Count > 0)
            {
                string? lang = lst.First().lang_code?.ToLower();
                //send contact mail to client
                string fileName = "CustomerSupport_" + lang + ".html";
                string htmlBody = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MailsTemp//", fileName));
                htmlBody = htmlBody.Replace("@user", client_name);
                MailData Mail_Data = new MailData { EmailToId = client_email, EmailToName = client_name, EmailSubject = UtilsCls.GetMailSubjectByLang(lang, 4), EmailBody = htmlBody };
                _mailSettingDao.SendMail(Mail_Data);
                response = new InvoiceResponse { success = true, idOut = 0 };
            }
            //exclude custom package from Invoice 
            var newList = lst.Where(wr => wr.is_custom == false).ToList();
            if (newList != null && newList.Count > 0)
            {
                int count = 0;
                decimal maxId = 0;
                decimal servicemaxId = 0;
                //concatenate each service code together + concatenate each package code together + date now to make unique and readable code
                //invoice code ex => WACO-BICO-20250810 => firstwo letter Service Code, second two letter Package Code + date
                string invCode = string.Join("-", newList.Select(e => String.IsNullOrEmpty(e.package_code) ? "00" : (String.IsNullOrEmpty(e.service_code) ? "" : e.service_code) + e.package_code )) + "-" + DateTime.Now.ToString("yyyyMMdd");

                //first save in InvoiceMain (make invoice)
                var totalPrice = newList.Sum(s => s.package_sale_price);
                var totalDiscount = newList.Sum(s => s.discount_amount);
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
                    copoun_id = 0,
                    grand_total_price = TotalPriceAfterTax,
                    status = 1, //mean pending
                    tax_id = 1

                };
                try
                {

                    if (_db.InvoiceMains.Count() > 0)
                    {
                        //check duplicate validation
                        var result = _db.InvoiceMains.Where(wr => wr.client_id == client_id && wr.active == main.active && wr.invoice_code == main.invoice_code && wr.invoice_date == main.invoice_date).SingleOrDefault();
                        if (result != null)
                        {
                            return new InvoiceResponse { success = false, errors = _localizer["DuplicateData"] };
                        }

                        maxId = _db.InvoiceMains.Max(d => d.invoice_id);



                    }
                    main.invoice_id = maxId + 1;
                    main.invoice_code_auto = "INV" + main.invoice_id;
                    _db.InvoiceMains.Add(main);
                    _db.SaveChanges();


                    response = new InvoiceResponse { success = true, idOut = main.invoice_id };
                }
                catch (Exception ex)
                {
                    response = new InvoiceResponse { errors = ex.Message, success = false, idOut = 0 };
                }
                // if invoice main sucess continue to save packages details
                if (response.success)
                {
                    //second save Client services List
                    foreach (InvoiceReq row in newList)
                    {

                        ClientService service = new ClientService { client_id = client_id, id = 0, productId = row.productId, package_id = row.package_id, invoice_id = response.idOut, active = true ,service_package_id=row.service_package_id};
                        if (_db.ClientServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.ClientServices.Where(wr => wr.client_id == service.client_id && wr.productId == service.productId && wr.package_id == service.package_id && wr.invoice_id == service.invoice_id && wr.active == true && wr.service_package_id==row.service_package_id).SingleOrDefault();
                            if (result != null)
                            {
                                return new InvoiceResponse { success = false, errors = _localizer["DuplicateData"] };
                            }

                            servicemaxId = _db.ClientServices.Max(d => d.id);


                        }
                        service.id = servicemaxId + 1;
                        _db.ClientServices.Add(service);
                        _db.SaveChanges();

                        count++;
                    }


                    if (count == newList.Count)
                    {
                        //get client profile data

                        ClientProfileCast profile = GetClientProfiles(client_id).Result.SingleOrDefault();

                        //start send invoice mail

                        response = new InvoiceResponse
                        {
                            errors = null,
                            success = true,
                            invoice = new HtmlInvoice
                            {
                                address = profile != null ? profile.address : "",
                                contact = profile != null ? profile.phone_number : "",
                                Discount = main.discount,
                                services = newList,
                                InvoiceNo = main.invoice_code,
                                IssuedDate = main.invoice_date?.ToString("yyyy-MM-dd"),
                                Total = main.total_price,
                                SubTtotal = main.total_price,
                                user = client_name

                            }

                        };
                    }
                    else
                    {
                        response = new InvoiceResponse { errors = _localizer["CheckAdmin"], success = false };
                    }
                }
            }
            
            return response;
        }
       
      

        //get feature list for specific service package
        public List<PackagesFeatureRes> getPackageFeatures(PackageFeatureReq req)
        {
            try
            {
                return  _db.packages_features.Where(wr => wr.service_package_id == req.service_package_id)
                                                  .Join(_db.main_features,
                                                         PKGF => new { PKGF.feature_id },
                                                         FEAT => new { feature_id = FEAT.id },
                                                         (PKGF, FEAT) => new { PKGF, FEAT }
                                                          )
                                                    .Join(_db.features_translations.Where(wr => wr.lang_code == req.lang_code),
                                                          combined => new { feature_id = combined.FEAT.id},
                                                          Trans => new { Trans.feature_id},
                                                          (combined, Trans) => new PackagesFeatureRes
                                                          {
                                                              feature_id = combined.PKGF.feature_id,
                                                              service_package_id = combined.PKGF.service_package_id,
                                                              id = combined.PKGF.id,
                                                              feature_code = combined.FEAT.feature_code,
                                                              feature_default_name = combined.FEAT.feature_default_name,
                                                              feature_description= Trans.feature_description,
                                                              feature_name= Trans.feature_name,
                                                              lang_code= Trans.lang_code
                                                          })
                                                  .ToList();
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        
        //get packages data by lang and currency group by service
        public async Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req)
        {
            try
            {
                var result = await _db.packagesdetailswithservices.Where(wr => wr.lang_code == req.lang &&  wr.curr_code.ToLower() == (wr.curr_code == null ? wr.curr_code : req.curr_code.ToLower()) && wr.active == true).ToListAsync();
                var fullEntries = result.Select(s => new PricingPackageCast
                {
                    service_id = (int)s.service_id,
                    curr_code = s.curr_code,
                    lang_code = s.lang_code,
                    package_sale_price = s.package_sale_price,
                    active = s.active,
                    discount_amount = s.discount_amount,
                    discount_type = (short?)s.discount_type,
                    package_price = s.package_price,
                    package_name = s.package_name,
                    package_id = (int)s.package_id,
                    package_desc = s.package_desc,
                    order = s.order,
                    package_details = s.package_details,
                    service_name = s.service_name,
                    is_recommend = s.is_recommend,
                    package_code = s.package_code,
                    isSelected = false,
                    is_custom = s.is_custom,
                    service_package_id=s.service_package_id,
                    service_code=s.service_code,
                    features = getPackageFeatures(new PackageFeatureReq { lang_code = req.lang,service_package_id=s.service_package_id }).ToList()

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

                    ClientService service = new ClientService { client_id = client_id, id = row.id, productId = row.productId,package_id=row.package_id,invoice_id=0,active=true,service_package_id=row.service_package_id };
                    if (service.id == 0)
                    {
                        if (_db.ClientServices.Count() > 0)
                        {
                            //check duplicate validation
                            var result = _db.ClientServices.Where(wr => wr.client_id == service.client_id && wr.productId == service.productId && wr.package_id == service.package_id && wr.active == true && wr.service_package_id == row.service_package_id).SingleOrDefault();
                            if (result != null)
                            {
                                return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
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
                    response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
                }
            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        public async Task<ClientService> CheckServiceSelected(int productId, int package_id, string clientId)
        {

            try
            {
                var result = await _db.ClientServices.Where(wr => wr.client_id == clientId && wr.package_id == (package_id == 0 ? wr.package_id : package_id) &&  wr.productId == productId && wr.active == true).SingleOrDefaultAsync();
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
       
        #endregion "packages &services"

    }
}

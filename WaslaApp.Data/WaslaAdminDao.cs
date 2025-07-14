using Mails_App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.admin.Packages_Services;
using WaslaApp.Data.Models.admin.Questions;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.invoices;
using WaslaApp.Data.Models.PackagesAndServices;

namespace WaslaApp.Data
{
    public class WaslaAdminDao
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly MailSettingDao _mailSettingDao;
        private readonly wasla_client_dbContext _db;

        public WaslaAdminDao(wasla_client_dbContext db, MailSettingDao mailSettingDao, IStringLocalizer<Messages> localizer)
        {
            _db = db;
            _mailSettingDao = mailSettingDao;
            _localizer = localizer;
        }

        #region "questions"
        //save main questions
        public ResponseCls SaveMainResigstraionQues(Main_RegistrationQuestion row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
               
                if (row.ques_id == 0)
                {
                    //check duplicate order (in add new row)
                    if (_db.Main_RegistrationQuestions.Where(wr => wr.order == row.order && wr.active == row.active).SingleOrDefault() != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateOrder"] };
                    }
                    //check duplicate validation
                    var result = _db.Main_RegistrationQuestions.Where(wr => wr.ques_title_default == row.ques_title_default && wr.active == row.active).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }
                   
                    if (_db.Main_RegistrationQuestions.Count() > 0)
                    {
                        maxId = _db.Main_RegistrationQuestions.Max(d => d.ques_id);

                    }
                    row.ques_id = maxId + 1;
                    _db.Main_RegistrationQuestions.Add(row);
                    _db.SaveChanges();
                }
                else
                {
                    //check duplicate order (in update row)
                    if (_db.Main_RegistrationQuestions.Where(wr => wr.order == row.order && wr.active == row.active && wr.ques_id != row.ques_id).SingleOrDefault() != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateOrder"] };
                    }
                    _db.Main_RegistrationQuestions.Update(row);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        //save questions with translations
        public ResponseCls SaveResigstraionQuesTranslations(RegistrationQuestions_TranslationSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                RegistrationQuestions_Translation ques = new RegistrationQuestions_Translation
                {
                     id = row.id,
                    lang_code =row.lang_code,
                    ques_id=row.ques_id,
                    ques_title= row.ques_title
                };
                if (row.delete == true)
                {
                    _db.Remove(ques);
                    _db.SaveChanges();
                    return new ResponseCls { errors = null, success = true };
                }
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.RegistrationQuestions_Translations.Where(wr => wr.ques_title == row.ques_title && wr.lang_code == row.lang_code && wr.ques_id == row.ques_id).SingleOrDefault();
                    if (result != null)
                    {
                        
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.RegistrationQuestions_Translations.Count() > 0)
                    {
                        maxId = _db.RegistrationQuestions_Translations.Max(d => d.id);

                    }
                    ques.id = maxId + 1;
                    _db.RegistrationQuestions_Translations.Add(ques);
                    _db.SaveChanges();
                }
                else
                {
                    _db.RegistrationQuestions_Translations.Update(ques);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }


        //get questions with different translations
        public async Task<List<QuestionsWithTranslationGrp>> getQuesWithTranslations()
        {
            try
            {

                //var result = await _db.RegistrationQuestions_Translations
                //                              .Join(_db.Main_RegistrationQuestions.Where(wr => wr.active == true),
                //                                     TRANS => new { TRANS.ques_id },
                //                                     QUES => new { QUES.ques_id },
                //                                     (TRANS, QUES) => new QuestionsWithTranslation
                //                                     {
                //                                         ques_id= QUES.ques_id,
                //                                         ques_title_default= QUES.ques_title_default,
                //                                         active= QUES.active,
                //                                         order= QUES.order,
                //                                         ques_title= TRANS.ques_title,
                //                                         ques_type= QUES.ques_type,
                //                                         id = TRANS.id,
                //                                         lang_code = TRANS.lang_code

                //                                     }
                //                                    )
                //                              .ToListAsync();
                var result  = await _db.registrationqueswithlangs.ToListAsync();
                return result.GroupBy(grp => new
                {
                    grp.ques_id,
                    grp.ques_title_default,
                    grp.order,
                    grp.active
                }).Select(s => new QuestionsWithTranslationGrp
                {
                   ques_id=s.Key.ques_id,
                   active= s.Key.active,
                   order= s.Key.order,
                   ques_title_default= s.Key.ques_title_default,
                    questions = result.Where(wr => wr.ques_id == s.Key.ques_id).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region services & packages

        //save main services data by admin
        public ResponseCls SaveMainServices(MServiceSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                main_service service = new main_service { id= row.id,active=row.active,default_name=row.default_name,service_code=row.service_code };
                if (service.id == 0)
                {
                    //check duplicate validation
                    var result = _db.main_services.Where(wr => wr.service_code == service.service_code && wr.active == service.active).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }
                    if (_db.main_services.Count() > 0)
                    {
                        maxId = _db.main_services.Max(d => d.id);

                    }
                    //    maxId = _db.Services.Max(d => d.productId);



                    service.id = maxId + 1;
                    _db.main_services.Add(service);
                    _db.SaveChanges();
                }
                else
                {
                    _db.main_services.Update(service);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }
        //save services with translations
        public ResponseCls SaveServicesTranslations(ServiceTranslationSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                
                service_translation service = new service_translation { service_id = row.service_id, id = row.id, product_desc = row.product_desc, productname = row.productname, lang_code = row.lang_code };
                if (row.delete == true)
                {
                    _db.Remove(service);
                    _db.SaveChanges();
                    return new ResponseCls { errors = null, success = true };
                }

                if (service.id == 0)
                {
                    //check duplicate validation
                    var result = _db.service_translations.Where(wr => wr.service_id == service.service_id && wr.productname == service.productname && wr.lang_code == service.lang_code).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.service_translations.Count() > 0)
                    {
                        maxId = _db.service_translations.Max(d => d.id);

                    }
                    service.id = maxId + 1;
                    _db.service_translations.Add(service);
                    _db.SaveChanges();
                }
                else
                {
                    _db.service_translations.Update(service);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        //service list with & without translation
        public async Task<List<main_service>> getMainServices(PackageAndServicesGetReq req)
        {
            try
            {
                if (req.isDropDown == true)
                {
                    //mean get main services without its translation used in dropdown
                    return await _db.main_services.Where(wr => wr.active == true).ToListAsync();
                }
                else
                {
                    //mean get main services with its translation used in grid
                    return await _db.main_services.Where(wr => wr.active == true)
                                                  .Include(i => i.service_translations)
                                                   .ToListAsync();
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //packages list with & without translation
        public async Task<List<package>> getMainPackages(PackageAndServicesGetReq req)
        {
            try
            {
                if (req.isDropDown == true)
                {
                    //mean get main packages without its translation used in dropdown
                    return await _db.packages.Where(wr => wr.active == true).ToListAsync();
                }
                else
                {
                    //mean get main packages with its translation used in grid
                    return await _db.packages.Where(wr => wr.active == true)
                    .Include(i => i.package_translations)
                                         .ToListAsync();
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //get price list for specific service package
        public async Task<List<service_package_price>> getServicePackagePrice(int service_package_id)
        {
            try
            {
                return await _db.service_package_prices.Where(wr => wr.service_package_id == service_package_id).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //save main package data by admin
        public ResponseCls SaveMainPackage(PackageSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                package pkg = new package
                {
                    order=row.order,
                    id=row.id,
                    active=row.active,
                    default_name=row.default_name,
                    is_custom=row.is_custom,
                    is_recommend=row.is_recommend,
                    package_code=row.package_code
                    
                };

               
                if (row.id == 0)
                {
                    //check duplicate order (in add new row )
                    if (_db.packages.Where(wr => wr.order == row.order && wr.active == row.active).SingleOrDefault() != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateOrder"] };
                    }
                    //check duplicate validation
                    var result = _db.packages.Where(wr => wr.package_code == row.package_code && wr.active == row.active).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.packages.Count() > 0)
                    {
                        maxId = _db.packages.Max(d => d.id);

                    }
                    pkg.id = maxId + 1;
                    _db.packages.Add(pkg);
                    _db.SaveChanges();
                }
                else
                {
                    //check duplicate order (in update row )
                    if (_db.packages.Where(wr => wr.order == row.order && wr.active == row.active && wr.id != row.id).SingleOrDefault() != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateOrder"] };
                    }
                    _db.packages.Update(pkg);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }
        //save packages with translations
        public ResponseCls SavePackageTranslations(PackageTranslationSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                package_translation package = new package_translation { lang_code=row.lang_code,id=row.id,package_desc=row.package_desc,package_details=row.package_details,package_id=row.package_id,package_name=row.package_name};
                if (row.delete == true)
                {
                    _db.Remove(package);
                    _db.SaveChanges();
                   return new ResponseCls { errors = null, success = true };
                }
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.package_translations.Where(wr =>  wr.package_id == row.package_id && wr.package_name == row.package_name && wr.lang_code == row.lang_code).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.package_translations.Count() > 0)
                    {
                        maxId = _db.package_translations.Max(d => d.id);

                    }
                    package.id = maxId + 1;
                    _db.package_translations.Add(package);
                    _db.SaveChanges();
                }
                else
                {
                    _db.package_translations.Update(package);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }
            return response;
        }

        //assign packages to service
        public ResponseCls AssignPackagesToService(ServicePackageReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                service_package service_Package= new service_package { id = row.id ,package_id=row.package_id,service_id=row.service_id,is_recommend=row.is_recommend};
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.service_packages.Where(wr => wr.service_id == row.service_id && wr.package_id == row.package_id).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    //    maxId = _db.Services.Max(d => d.productId);



                    //service.productId = maxId + 1;
                    _db.service_packages.Add(service_Package);
                    _db.SaveChanges();
                }
                else
                {
                    _db.service_packages.Update(service_Package);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        //assign price with currency to package
        public ResponseCls AssignPriceToPackage(PackagePriceSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                service_package_price price = new service_package_price
                {
                    id=row.id,
                    curr_code=row.curr_code,
                    discount_amount=row.discount_amount,
                    discount_type= row.discount_type,
                    package_price= row.package_price,
                    package_sale_price= row.package_sale_price,
                    service_package_id=row.service_package_id
                };
             
                if (row.delete)
                {
                    _db.Remove(price);
                    _db.SaveChanges();
                  return  new ResponseCls { errors = null, success = true };
                }
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.service_package_prices.Where(wr => wr.service_package_id == row.service_package_id && wr.curr_code == row.curr_code).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    //    maxId = _db.Services.Max(d => d.productId);



                    //service.productId = maxId + 1;
                    _db.service_package_prices.Add(price);
                    _db.SaveChanges();
                }
                else
                {
                    _db.service_package_prices.Update(price);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        public List<ServiceGrpWithPkgs> getServiceGrpWithPkgs()
        {
            try
            {
               var result = _db.service_packages.Join(
                                        _db.packages.Where(wr => wr.active == true),
                                        SP => new { SP.package_id, },
                                        PKG => new { package_id = PKG.id },
                                        (SP, PKG) => new { SP, PKG }
                                     )
                                    .Join(
                                       _db.main_services.Where(wr=> wr.active == true),
                                        SP_PKG => new { SP_PKG.SP.service_id, },
                                        SERV => new { service_id = SERV.id },
                                        (combinedEntry, SERV) => new ServicePkgsWithDetails
                                        {
                                            service_id = combinedEntry.SP.service_id,
                                            package_id= combinedEntry.SP.package_id,
                                            service_package_id= combinedEntry.SP.id,
                                            service_code= SERV.service_code,
                                            package_code= combinedEntry.PKG.package_code,
                                            is_custom= combinedEntry.PKG.is_custom,
                                            is_recommend= combinedEntry.SP.is_recommend,
                                            package_default_name= combinedEntry.PKG.default_name,
                                            service_default_name= SERV.default_name,
                                            order= combinedEntry.PKG.order
                                        }
                                    ).ToList();

                return  result.GroupBy(grp => new
                {
                    grp.service_default_name,
                    grp.service_code,
                    grp.service_id,
                }).Select(s => new ServiceGrpWithPkgs
                {
                    service_id = s.Key.service_id,
                    service_default_name = s.Key.service_default_name,
                    service_code = s.Key.service_code,
                    pkgs = result.Where(wr => wr.service_id == s.Key.service_id).ToList()
                }).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //features dropdown
        public async Task<List<main_feature>> getMainFeatures()
        {
            try
            {
               
                    return await _db.main_features.Where(wr => wr.active == true).ToListAsync();
               
         
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //get features with different translations
        public async Task<List<FeaturesWithTranslationGrp>> getFeaturesWithTranslations()
        {
            try
            {

                //var result =  await _db.features_translations
                //                              .Join(_db.main_features.Where(wr => wr.active == true),
                //                                     TRANS => new { TRANS.feature_id },
                //                                     FEAT => new { feature_id = FEAT.id },
                //                                     (TRANS, FEAT) => new FeaturesWithTranslation
                //                                     {
                //                                         feature_code = FEAT.feature_code,
                //                                         feature_default_name= FEAT.feature_default_name,
                //                                         feature_description = TRANS.feature_description,
                //                                         feature_name= TRANS.feature_name,
                //                                         feature_id= TRANS.feature_id,
                //                                         id= TRANS.id,
                //                                         lang_code= TRANS.lang_code

                //                                     }
                //                                    )
                //                              .ToListAsync();
                var result = await _db.featureswithtranslations.ToListAsync();
                return result.GroupBy(grp => new
                {
                    grp.feature_id,
                    grp.feature_code,
                    grp.feature_default_name
                }).Select(s => new FeaturesWithTranslationGrp
                {
                   feature_default_name=s.Key.feature_default_name,
                   feature_code=s.Key.feature_code,
                   feature_id=s.Key.feature_id,
                   features_Translations = result.Where(wr => wr.feature_id == s.Key.feature_id && wr.id != null).ToList()
                }).ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        //get feature list for specific service-package
        public async Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req)
        {
            try
            {
                return await _db.packages_features.Where(wr => wr.service_package_id == req.service_package_id)
                                                  .Join(_db.main_features,
                                                         PKGF => new { PKGF.feature_id },
                                                         FEAT => new { feature_id = FEAT.id},
                                                         (PKGF, FEAT) => new PackagesFeatureRes
                                                         {
                                                             feature_id= PKGF.feature_id,
                                                             service_package_id = PKGF.service_package_id,
                                                             id= PKGF.id,
                                                             feature_code= FEAT.feature_code,
                                                             feature_default_name= FEAT.feature_default_name

                                                         })
                                                  .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //assign features to package
        public ResponseCls AssignFeaturesToPackage(PkgFeatureSaveDelete row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                packages_feature feat = new packages_feature { feature_id=row.feature_id,id=row.id, service_package_id = row.service_package_id };
                if (row.delete)
                {
                    _db.Remove(feat);
                    _db.SaveChanges();
                    return new ResponseCls { errors = null, success = true };
                }
                
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.packages_features.Where(wr => wr.service_package_id == row.service_package_id && wr.feature_id == row.feature_id).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }
                    _db.packages_features.Add(feat);
                    _db.SaveChanges();
                }
                else
                {
                    _db.packages_features.Update(feat);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        //save main feature 
        public ResponseCls SaveMainFeature(MainFeatureSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                if (row.delete == true)
                {
                    _db.Remove(row);
                    _db.SaveChanges();
                    return new ResponseCls { errors = null, success = true };
                }
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.main_features.Where(wr => wr.feature_code == row.feature_code && wr.active == row.active).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.main_features.Count() > 0)
                    {
                        maxId = _db.main_features.Max(d => d.id);

                    }
                    row.id = maxId + 1;
                    _db.main_features.Add(row);
                    _db.SaveChanges();
                }
                else
                {
                    _db.main_features.Update(row);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };


            }

            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }

            return response;
        }

        //save feature with translations
        public ResponseCls SaveFeatureTranslations(FeaturesTranslationSaveReq row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
               
                if (row.delete == true)
                {
                    _db.Remove(row);
                    _db.SaveChanges();
                    return new ResponseCls { errors = null, success = true };
                }
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.features_translations.Where(wr => wr.feature_id == row.feature_id && wr.feature_name == row.feature_name && wr.lang_code == row.lang_code).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    if (_db.features_translations.Count() > 0)
                    {
                        maxId = _db.features_translations.Max(d => d.id);

                    }
                    row.id = maxId + 1;
                    _db.features_translations.Add(row);
                    _db.SaveChanges();
                }
                else
                {
                    _db.features_translations.Update(row);
                    _db.SaveChanges();
                }

                response = new ResponseCls { errors = null, success = true };
            }
            catch (Exception ex)
            {
                response = new ResponseCls { errors = _localizer["CheckAdmin"], success = false };
            }
            return response;
        }
        #endregion

    }
}

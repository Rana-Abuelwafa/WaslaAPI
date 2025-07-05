﻿using Mails_App;
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

        //service dropdown
        public async Task<List<main_service>> getMainServices(PackageAndServicesGetReq req)
        {
            try
            {
                if (req.isDropDown == true)
                {
                    return await _db.main_services.Where(wr => wr.active == true).ToListAsync();
                }
                else
                {
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

        //packages dropdown
        public async Task<List<package>> getMainPackages(PackageAndServicesGetReq req)
        {
            try
            {
                if (req.isDropDown == true)
                {
                    return await _db.packages.Where(wr => wr.active == true).ToListAsync();
                }
                else
                {
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

                var result =  await _db.features_translations
                                              .Join(_db.main_features.Where(wr => wr.active == true),
                                                     TRANS => new { TRANS.feature_id },
                                                     FEAT => new { feature_id = FEAT.id },
                                                     (TRANS, FEAT) => new FeaturesWithTranslation
                                                     {
                                                         feature_code = FEAT.feature_code,
                                                         feature_default_name= FEAT.feature_default_name,
                                                         feature_description = TRANS.feature_description,
                                                         feature_name= TRANS.feature_name,
                                                         feature_id= TRANS.feature_id,
                                                         id= TRANS.id,
                                                         lang_code= TRANS.lang_code

                                                     }
                                                    )
                                              .ToListAsync();
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
                   features_Translations = result.Where(wr => wr.feature_id == s.Key.feature_id).ToList()
                }).ToList();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        //get feature list for specific service package
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

        //save packages with translations
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

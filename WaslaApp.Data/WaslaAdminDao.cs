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
        public ResponseCls SaveMainServices(main_service service)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                if (service.id == 0)
                {
                    //check duplicate validation
                    var result = _db.main_services.Where(wr => wr.service_code == service.service_code && wr.active == service.active).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    //    maxId = _db.Services.Max(d => d.productId);



                    //service.productId = maxId + 1;
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
        public ResponseCls SaveServices(service_translation service)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                if (service.id == 0)
                {
                    //check duplicate validation
                    var result = _db.service_translations.Where(wr => wr.service_id == service.service_id && wr.productname == service.productname).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    //    maxId = _db.Services.Max(d => d.productId);



                    //service.productId = maxId + 1;
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
        public async Task<List<main_service>> getMainServices()
        {
            try
            {
                return await _db.main_services.Where(wr => wr.active == true).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //packages dropdown
        public async Task<List<package>> getMainPackages()
        {
            try
            {
                return await _db.packages.Where(wr => wr.active == true).ToListAsync();
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
        //save main services data by admin
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

                    //    maxId = _db.Services.Max(d => d.productId);



                    //service.productId = maxId + 1;
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
        //save services with translations
        public ResponseCls SavePackageTranslations(package_translation row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.package_translations.Where(wr =>  wr.package_id == row.package_id && wr.package_name == row.package_name).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }

                    //    maxId = _db.Services.Max(d => d.productId);
                    //service.productId = maxId + 1;
                    _db.package_translations.Add(row);
                    _db.SaveChanges();
                }
                else
                {
                    _db.package_translations.Update(row);
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
                service_package service_Package= new service_package { id = row.id ,package_id=row.package_id,service_id=row.service_id};
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
                                            is_recommend= combinedEntry.PKG.is_recommend,
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

        //get feature list for specific service package
        public async Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req)
        {
            try
            {
                return await _db.packages_features.Where(wr => wr.package_id == req.package_id)
                                                  .Join(_db.main_features,
                                                         PKGF => new { PKGF.feature_id },
                                                         FEAT => new { feature_id = FEAT.id},
                                                         (PKGF, FEAT) => new PackagesFeatureRes
                                                         {
                                                             feature_id= PKGF.feature_id,
                                                             package_id= PKGF.package_id,
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
        public ResponseCls AssignFeaturesToPackage(packages_feature row)
        {
            ResponseCls response;
            int maxId = 0;
            try
            {
                
                if (row.id == 0)
                {
                    //check duplicate validation
                    var result = _db.packages_features.Where(wr => wr.package_id == row.package_id && wr.feature_id == row.feature_id).SingleOrDefault();
                    if (result != null)
                    {
                        return new ResponseCls { success = false, errors = _localizer["DuplicateData"] };
                    }
                    _db.packages_features.Add(row);
                    _db.SaveChanges();
                }
                else
                {
                    _db.packages_features.Update(row);
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

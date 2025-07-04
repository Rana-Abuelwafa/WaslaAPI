﻿using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.admin.Packages_Services;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.PackagesAndServices;

namespace Wasla_App.services.Admin
{
    public interface IAdminWaslaService
    {
        public ResponseCls SaveMainFeature(MainFeatureSaveReq row);
        public ResponseCls SaveFeatureTranslations(FeaturesTranslationSaveReq row);
        public  Task<List<FeaturesWithTranslationGrp>> getFeaturesWithTranslations();
        public ResponseCls SaveMainServices(MServiceSaveReq service);
        public ResponseCls SaveServicesTranslations(ServiceTranslationSaveReq service);
        public  Task<List<main_service>> getMainServices(PackageAndServicesGetReq req);
        public Task<List<package>> getMainPackages(PackageAndServicesGetReq req);
        public Task<List<service_package_price>> getServicePackagePrice(int service_package_id);
        public ResponseCls SaveMainPackage(PackageSaveReq row);
        public ResponseCls SavePackageTranslations(PackageTranslationSaveReq row);
        public ResponseCls AssignPackagesToService(ServicePackageReq row);
        public ResponseCls AssignPriceToPackage(PackagePriceSaveReq row);
        public List<ServiceGrpWithPkgs> getServiceGrpWithPkgs();

        public Task<List<main_feature>> getMainFeatures();
        public Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req);
        public ResponseCls AssignFeaturesToPackage(PkgFeatureSaveDelete row);
    }
}

using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.PackagesAndServices;

namespace Wasla_App.services.Admin
{
    public interface IAdminWaslaService
    {
        //public ResponseCls SaveMainServices(main_service service);
        //public ResponseCls SaveServices(service_translation service);
        public  Task<List<main_service>> getMainServices();
        public Task<List<package>> getMainPackages();
        public Task<List<service_package_price>> getServicePackagePrice(int service_package_id);
        public ResponseCls SaveMainPackage(PackageSaveReq row);
        //public ResponseCls SavePackageTranslations(package_translation row);
        public ResponseCls AssignPackagesToService(ServicePackageReq row);
        public ResponseCls AssignPriceToPackage(PackagePriceSaveReq row);
        public List<ServiceGrpWithPkgs> getServiceGrpWithPkgs();

        public Task<List<main_feature>> getMainFeatures();
        public Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req);
        public ResponseCls AssignFeaturesToPackage(packages_feature row);
    }
}

using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.PackagesAndServices;

namespace Wasla_App.services.Admin
{
    public class AdminWaslaService : IAdminWaslaService
    {
        private WaslaAdminDao _waslaAdminDao;

        public AdminWaslaService(WaslaAdminDao waslaAdminDao)
        {
            _waslaAdminDao = waslaAdminDao;

        }

        public ResponseCls AssignFeaturesToPackage(packages_feature row)
        {
            return _waslaAdminDao.AssignFeaturesToPackage(row);
        }

        public ResponseCls AssignPackagesToService(ServicePackageReq row)
        {
            return _waslaAdminDao.AssignPackagesToService(row);
        }

        public ResponseCls AssignPriceToPackage(PackagePriceSaveReq row)
        {
            return _waslaAdminDao.AssignPriceToPackage(row);
        }

        public Task<List<main_feature>> getMainFeatures()
        {
            return _waslaAdminDao.getMainFeatures();
        }

        public Task<List<package>> getMainPackages()
        {
            return _waslaAdminDao.getMainPackages();
        }

        public Task<List<main_service>> getMainServices()
        {
            return _waslaAdminDao.getMainServices();
        }

        public Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req)
        {
            return _waslaAdminDao.getPackageFeatures(req);  
        }

        public List<ServiceGrpWithPkgs> getServiceGrpWithPkgs()
        {
            return _waslaAdminDao.getServiceGrpWithPkgs();
        }

        public Task<List<service_package_price>> getServicePackagePrice(int service_package_id)
        {
            return _waslaAdminDao.getServicePackagePrice(service_package_id);
        }

        public ResponseCls SaveMainPackage(PackageSaveReq row)
        {
            return _waslaAdminDao.SaveMainPackage(row);
        }
    }
}

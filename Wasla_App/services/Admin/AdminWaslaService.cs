using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.admin.Packages_Services;
using WaslaApp.Data.Models.admin.Questions;
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

        public ResponseCls AssignFeaturesToPackage(PkgFeatureSaveDelete row)
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

        public Task<List<FeaturesWithTranslationGrp>> getFeaturesWithTranslations()
        {
            return _waslaAdminDao.getFeaturesWithTranslations();
        }

        public Task<List<main_feature>> getMainFeatures()
        {
            return _waslaAdminDao.getMainFeatures();
        }

        public Task<List<package>> getMainPackages(PackageAndServicesGetReq req)
        {
            return _waslaAdminDao.getMainPackages(req);
        }

        public Task<List<main_service>> getMainServices(PackageAndServicesGetReq req)
        {
            return _waslaAdminDao.getMainServices(req);
        }

        public Task<List<PackagesFeatureRes>> getPackageFeatures(PackageFeatureReq req)
        {
            return _waslaAdminDao.getPackageFeatures(req);  
        }

        public Task<List<QuestionsWithTranslationGrp>> getQuesWithTranslations()
        {
            return _waslaAdminDao.getQuesWithTranslations();
        }

        public List<ServiceGrpWithPkgs> getServiceGrpWithPkgs()
        {
            return _waslaAdminDao.getServiceGrpWithPkgs();
        }

        public Task<List<service_package_price>> getServicePackagePrice(int service_package_id)
        {
            return _waslaAdminDao.getServicePackagePrice(service_package_id);
        }

        public ResponseCls SaveFeatureTranslations(FeaturesTranslationSaveReq row)
        {
            return _waslaAdminDao.SaveFeatureTranslations(row);
        }

        public ResponseCls SaveMainFeature(MainFeatureSaveReq row)
        {
            return _waslaAdminDao.SaveMainFeature(row);
        }

        public ResponseCls SaveMainPackage(PackageSaveReq row)
        {
            return _waslaAdminDao.SaveMainPackage(row);
        }

        public ResponseCls SaveMainResigstraionQues(Main_RegistrationQuestion row)
        {
            return _waslaAdminDao.SaveMainResigstraionQues(row);
        }

        public ResponseCls SaveMainServices(MServiceSaveReq service)
        {
            return _waslaAdminDao.SaveMainServices(service);
        }

        public ResponseCls SavePackageTranslations(PackageTranslationSaveReq row)
        {
            return _waslaAdminDao.SavePackageTranslations(row);
        }

        public ResponseCls SaveResigstraionQuesTranslations(RegistrationQuestions_Translation row)
        {
            return _waslaAdminDao.SaveResigstraionQuesTranslations(row);
        }

        public ResponseCls SaveServicesTranslations(ServiceTranslationSaveReq service)
        {
            return _waslaAdminDao.SaveServicesTranslations(service);
        }
    }
}

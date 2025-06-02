using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.services
{
    public class WaslaService : IWaslaService
    {
        private WaslaDAO _waslaDao;

        public WaslaService(WaslaDAO waslaDao)
        {
            _waslaDao = waslaDao;

        }

        public ResponseCls deleteQuestions(RegistrationQuestion ques)
        {
            return _waslaDao.deleteQuestions(ques);
        }

        public Task<List<ClientBrand>> GetClientBrands(string clientId)
        {
            return _waslaDao.GetClientBrands(clientId);
        }

        public Task<List<ClientImage>> GetProfileImage(string clientId)
        {
            return _waslaDao.GetProfileImage(clientId);
        }

        public Task<List<ClientProfileCast>> GetClientProfiles(string clientId)
        {
            return _waslaDao.GetClientProfiles(clientId);
        }

        public Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return _waslaDao.GetPaymentMethods();
        }

        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {
            return _waslaDao.getRegistrationQuestionList(lang);
        }

        public ResponseCls saveClientBrand(ClientBrand brand)
        {
            return _waslaDao.saveClientBrand(brand);
        }

        public ResponseCls saveProfileImage(ClientImage image)
        {
            return _waslaDao.saveProfileImage(image).Result;
        }

        public ResponseCls saveMainProfile(ClientProfileCast profile)
        {
            return _waslaDao.saveMainProfile(profile);
        }

        public ResponseCls saveQuestions(RegistrationQuestion ques)
        {
            return _waslaDao.saveQuestions(ques);
        }

        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId,string FullName,string email)
        {
            return _waslaDao.saveRegistrationSteps(lst, clientId, FullName, email);
        }

        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang)
        {
            return _waslaDao.getQuesWithAnswers(clientId, lang);
        }

        public ResponseCls saveClientCopoun(ClientCopoun copoun)
        {
            return _waslaDao.saveClientCopoun(copoun);
        }

        public Task<List<Service_Tree>> GetProduct_Tree(string clientId,string lang)
        {
           return _waslaDao.GetProduct_Tree(clientId,lang);
        }

        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id)
        {
            return _waslaDao.saveClientServices(lst, client_id);
        }

        public ResponseCls SaveProduct(Service service)
        {
            return _waslaDao.SaveProduct(service);
        }

        public Task<List<Service>> GetProduct(ServiceReq req)
        {
            return _waslaDao.GetProduct(req);
        }

        public Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req)
        {
            return _waslaDao.GetPricingPackageWithService(req);
        }

        public ResponseCls SavePricingPackage(PricingPackage package)
        {
            return _waslaDao.SavePricingPackage(package);
        }

        public ResponseCls SavePricingPKgFeatureLst(List<PricingPkgFeature> lst)
        {
            return _waslaDao.SavePricingPKgFeatureLst(lst);
        }

        public List<PricingPkgFeature> GetPricingPkgFeatures(PricingPkgFeatureReq req)
        {
            return _waslaDao.GetPricingPkgFeatures(req);
        }
    }
}

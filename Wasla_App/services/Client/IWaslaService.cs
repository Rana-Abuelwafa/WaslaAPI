using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.invoices;
using WaslaApp.Data.Models.PackagesAndServices;
using WaslaApp.Data.Models.profile;

namespace Wasla_App.services.Client
{
    public interface IWaslaService
    {
        #region "registration questions"
        public ResponseCls saveClientCopoun(ClientCopoun copoun);
        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang);
        public ResponseCls saveQuestions(RegistrationQuestion ques);
        public ResponseCls deleteQuestions(RegistrationQuestion ques);
        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang);
        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId,string FullName,string email);

        #endregion

        #region "profile"
        public ResponseCls UpdateInvoicePrices(InvUpdatePriceReq req, string client_id);
        public ResponseCls RemoveInvoice(InvRemoveReq req, string client_id);
        public ResponseCls CheckoutInvoice(CheckoutReq req, string client_id);
        public Task<List<ClientInvoiceGrp>> GetInvoicesByClient(string client_id);
        public Task<ClientCopounCast> ValidateClientCopoun(ClientCopounReq req,string client_id);
        public  Task<List<PaymentMethod>> GetPaymentMethods();
        public Task<List<ClientBrand>> GetClientBrands(string clientId);
        public Task<List<ClientProfileCast>> GetClientProfiles(string clientId);
        public ResponseCls saveMainProfile(ClientProfileCast profile);
        public ResponseCls saveClientBrand(ClientBrand brand);
        public ResponseCls saveProfileImage(ClientImage image);
        public Task<List<ClientImage>> GetProfileImage(string clientId);
        #endregion

        #region "packages & services"
       
        public InvoiceResponse MakeClientInvoiceForPackages(List<InvoiceReq> lst, string client_id, string client_name, string client_email);
        public Task<List<PricingPackageWithService>> GetPricingPackages(PricingPackageReq req);
        public ResponseCls SavePricingPKgFeatureLst(List<PricingPkgFeature> lst);
        public Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req);
        public List<PricingPkgFeature> GetPricingPkgFeatures(PricingPkgFeatureReq req);
        public ResponseCls SavePricingPackage(PricingPackage package);
        public Task<List<Service>> GetProduct(ServiceReq req);
        public ResponseCls SaveProduct(Service service);
        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id);
        public Task<List<Service_Tree>> GetProduct_Tree(string clientId, string lang);
        #endregion

    }
}

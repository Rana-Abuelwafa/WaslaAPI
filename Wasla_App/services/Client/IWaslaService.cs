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
        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang);
        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId,string FullName,string email);

        #endregion

        #region "profile"
        public ResponseCls UpdateInvoicePrices(InvUpdatePriceReq req, string client_id);
        public ResponseCls RemoveInvoice(InvRemoveReq req, string client_id);
        public ResponseCls CheckoutInvoice(CheckoutReq req, string client_id, string client_name,string completeprofile);
        public Task<List<ClientInvoiceGrp>> GetInvoicesByClient(ClientInvoiceReq req ,string client_id);
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
        public Task<List<ServicesWithPkg>> GetSearchResult(SearchCls req);
        public InvoiceResponse MakeClientInvoiceForPackages(List<InvoiceReq> lst, string client_id, string client_name, string client_email);
        public Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req);
        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id);
        #endregion

    }
}

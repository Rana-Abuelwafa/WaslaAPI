
using WaslaApp.Data;
using WaslaApp.Data.Entities;
using WaslaApp.Data.Models.global;
using WaslaApp.Data.Models.invoices;
using WaslaApp.Data.Models.PackagesAndServices;
using WaslaApp.Data.Models.profile;

namespace Wasla_App.services.Client
{
    public class WaslaService : IWaslaService
    {
        private WaslaDAO _waslaDao;

        public WaslaService(WaslaDAO waslaDao)
        {
            _waslaDao = waslaDao;

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

      

        public ResponseCls saveClientServices(List<ClientServiceCast> lst, string client_id)
        {
            return _waslaDao.saveClientServices(lst, client_id);
        }

 

        public Task<List<ServicesWithPkg>> GetPricingPackageWithService(LangReq req)
        {
            return _waslaDao.GetPricingPackageWithService(req);
        }


        public InvoiceResponse MakeClientInvoiceForPackages(List<InvoiceReq> lst, string client_id, string client_name, string client_email)
        {
            return _waslaDao.MakeClientInvoiceForPackages(lst,client_id,client_name,client_email);
        }

        public Task<ClientCopounCast> ValidateClientCopoun(ClientCopounReq req, string client_id)
        {
            return _waslaDao.ValidateClientCopoun(req,client_id);
        }

        public Task<List<ClientInvoiceGrp>> GetInvoicesByClient(ClientInvoiceReq req,string client_id)
        {
            return _waslaDao.GetInvoicesByClient(req,client_id);
        }

        public ResponseCls CheckoutInvoice(CheckoutReq req, string client_id, string client_name,string completeprofile)
        {
            return _waslaDao.CheckoutInvoice(req,client_id,client_name, completeprofile);
        }

        public ResponseCls RemoveInvoice(InvRemoveReq req, string client_id)
        {
            return _waslaDao.RemoveInvoice(req,client_id);
        }

        public ResponseCls UpdateInvoicePrices(InvUpdatePriceReq req, string client_id)
        {
            return _waslaDao.UpdateInvoicePrices(req,client_id);
        }
    }
}

using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.services
{
    public interface IWaslaService
    {
        #region "registration questions"
        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang);
        public ResponseCls saveQuestions(RegistrationQuestion ques);
        public ResponseCls deleteQuestions(RegistrationQuestion ques);
        public List<QuesWithAnswers> getQuesWithAnswers(string clientId, string lang);
        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId,string FullName,string email);

        #endregion

        #region "profile"
        public  Task<List<PaymentMethod>> GetPaymentMethods();
        public Task<List<ClientBrand>> GetClientBrands(string clientId);
        public Task<List<ClientProfile>> GetClientProfiles(string clientId);
        public ResponseCls saveMainProfile(ClientProfile profile);
        public ResponseCls saveClientBrand(ClientBrand brand);
        public ResponseCls saveProfileImage(ClientImage image);
        public Task<List<ClientImage>> GetProfileImage(string clientId);
        #endregion

    }
}

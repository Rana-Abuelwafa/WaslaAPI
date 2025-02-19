using WaslaApp.Data.Entities;
using WaslaApp.Data.Models;

namespace Wasla_App.services
{
    public interface IWaslaService
    {
        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang);
        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId);
    }
}

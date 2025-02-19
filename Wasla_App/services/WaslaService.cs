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

        public Task<List<RegistrationQuestion>> getRegistrationQuestionList(string lang)
        {
            return _waslaDao.getRegistrationQuestionList(lang);
        }

        public RegsistrationQuesResponse saveRegistrationSteps(List<RegistrationAnswer> lst,string clientId)
        {
            return _waslaDao.saveRegistrationSteps(lst, clientId);
        }
    }
}

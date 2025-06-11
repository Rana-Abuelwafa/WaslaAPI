
using Mails_App;

namespace Wasla_App.Services
{
    public class MailService : IMailService
    {
        //MailSettings Mail_Settings = null;
        private MailSettingDao _mailSettingDao;
        public MailService(MailSettingDao mailSettingDao)
        {
            _mailSettingDao = mailSettingDao;
        }

        public bool SendMail(MailData Mail_Data)
        {
            return _mailSettingDao.SendMail(Mail_Data);
        }
    }
}

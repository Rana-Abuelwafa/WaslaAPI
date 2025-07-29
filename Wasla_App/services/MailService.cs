
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

        public bool ContactMail(MailData Mail_Data, string sender)
        {
            return _mailSettingDao.ContactMail(Mail_Data, sender);
        }

        public bool SendMail(MailData Mail_Data)
        {
            return _mailSettingDao.SendMail(Mail_Data);
        }
    }
}

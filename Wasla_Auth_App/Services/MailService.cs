using MailKit.Net.Smtp;
using Mails_App;
using Microsoft.Extensions.Options;
using MimeKit;
//using System.Net.Mail;
using System.Net;
//using System.Net.Mail;
using System.Text;
using Wasla_Auth_App.Models;

namespace Wasla_Auth_App.Services
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

        //public bool SendOTPMail(MailData Mail_Data)
        //{
        //    return _mailSettingDao.SendMail(Mail_Data);
        //}
    }
}

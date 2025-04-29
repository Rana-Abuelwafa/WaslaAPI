using Wasla_Auth_App.Models;

namespace Wasla_Auth_App.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
        bool SendOTPMail(MailData Mail_Data);
        
    }

}

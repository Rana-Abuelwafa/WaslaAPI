

using Mails_App;

namespace Wasla_App.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
        bool ContactMail(MailData Mail_Data,string sender);
        
    }

}

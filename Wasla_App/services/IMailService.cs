﻿

using Mails_App;

namespace Wasla_App.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
        //bool SendOTPMail(MailData Mail_Data);
        
    }

}

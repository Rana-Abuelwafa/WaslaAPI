﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mails_App
{
    public class UtilsCls
    {
       
        public static string GetMailSubjectByLang(string lang,int type)
        {
            if (type == 1)
            //mean confirmation mail
            {
                if (lang == "ar")
                    return "مرحباً بك في وصلة";
                else if (lang == "en")
                    return "Welcome to Waslaa !";
                else if (lang == "de")
                    return "Willkommen bei Waslaa";
                else return "";
            }
            else if (type == 2)
            {
                //mean otp verify
                if (lang == "ar")
                    return "تأكيد البريد الإلكتروني-وصلة";
                else if (lang == "en")
                    return "Waslaa - Verify Your Email";
                else if (lang == "de")
                    return "Waslaa - Bestätigen Sie Ihre E-Mail";
                else return "";
            }
            else return "";
            
        }
    }
}

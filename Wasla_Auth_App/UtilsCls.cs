using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla_Auth_App
{
    public class UtilsCls
    {
        //this type config is static in code,
        //1 = confirmation mail ,
        //2 = otp verify
        //3 = invoice mail
        //4 = CUSTOMER SUPPORT mail

        public static string GetMailSubjectByLang(string lang, int type)
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
            else if (type == 3)
            {
                //mean invoice
                if (lang == "ar")
                    return "   فاتوره - وصلة";
                else if (lang == "en")
                    return "Waslaa - Packages' Invoice";
                else if (lang == "de")
                    return "Waslaa - Factuur van pakketten";
                else return "";
            }
            else if (type == 4)
            {
                //mean invoice
                if (lang == "ar")
                    return "   خدمة عملاء - وصلة";
                else if (lang == "en")
                    return "Waslaa - Customer Support";
                else if (lang == "de")
                    return "Waslaa - Klantenondersteuning";
                else return "";
            }
            else if (type == 5)
            {
                //mean checkout notify to customer care
                if (lang == "ar")
                    return "   اخطار الدفع - وصلة";
                else if (lang == "en")
                    return "Waslaa - Checkout Notify";
                else if (lang == "de")
                    return "Waslaa - Checkout-Benachrichtigung";
                else return "";
            }
            else return "";

        }
    }
}

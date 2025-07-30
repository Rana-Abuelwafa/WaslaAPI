using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data
{
    public class HelperCls
    {
        public static string getResponseMsg(string lang)
        {
            string? response = null;
            if(lang == "ar")
            {
                response = "<p>عزيزي العميل، لقد تم إرسال النموذج إلى مصممينا للمراجعة والتنفيذ، وسيتم الاتصال بك للحصول على مزيد من التفاصيل.<br/>نشكرك لاختيارك خدماتنا.<p/>";
            }else if(lang == "de")
            {
                response = "<p>Geachte klant, het formulier is ter beoordeling en implementatie naar onze ontwerpers verzonden. Wij nemen contact met u op voor meer informatie.<br/> Bedankt dat u voor onze diensten hebt gekozen.</p>";
            }
            else
            {
                response = "<p>Dear customer, the form has been submitted to our designers for review and implementation, you will be contacted to obtain further details.<br/><br/>Thank you for choosing our services.<p/>";
            }
            return response;
        }
        public static string getCopounText()
        {
            Random res = new Random();

            // String that contain both alphabets and numbers 
            String str = "Acdefghijklmnopqrstuvwxyz0123456789";
            int size = 4;

            // Initializing the empty string 
            String randomstring = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly 
                int x = res.Next(str.Length);

                // Appending the character at the  
                // index to the random alphanumeric string. 
                randomstring = randomstring + str[x];
            }
            return "OP" + randomstring.ToUpper();
        }

        
        
    }
}

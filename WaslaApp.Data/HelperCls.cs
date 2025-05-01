using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaslaApp.Data
{
    public class HelperCls
    {
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
            return "OP"+randomstring.ToUpper();
        }
        }
    }

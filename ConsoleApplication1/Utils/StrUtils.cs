using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrpParser.Utils
{
    class StrUtils
    {
        public static readonly int MAX_PATH_LEN = 200;
        public static string limitStr(string input)
        {
            if((input != null) && (input.Length > (MAX_PATH_LEN-Environment.CurrentDirectory.Length)))
            {
                
                return input.Substring(0, MAX_PATH_LEN - Environment.CurrentDirectory.Length);
            }
            else
            {
                return input;
            }
        }
    }
}

using System;

namespace CrpParser.Utils
{
    class StrUtils
    {
        public static readonly Int32 MAX_PATH_LEN = 200;
        public static String limitStr(String input)
        {
            if ((input != null) && (input.Length > (MAX_PATH_LEN - Environment.CurrentDirectory.Length)))
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

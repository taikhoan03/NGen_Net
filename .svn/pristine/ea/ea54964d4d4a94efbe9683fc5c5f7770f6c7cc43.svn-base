using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataObject.Util
{
    public static class StringUlti
    {
        public static string TrimAllWhiteSpace(this string obj)
        {
            if (obj.GetType() == typeof(String))
            {
                obj = Regex.Replace(obj.ToString(), @"\s+", " ");
                return obj.Trim(' ');
            }
            return null;
        }
    }
}

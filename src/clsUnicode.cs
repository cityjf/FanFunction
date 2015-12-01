using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FanFunction
{
    public class clsUnicode
    {
        /// <summary>
        /// 转换成Unicode码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }
        /// <summary>
        /// 解析Unicode码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decode(string s)
        {
            Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return reUnicode.Replace(s, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }
        /// <summary>
        /// 解析Unicode码，字符串必须全是由4位的Unicode组成
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecodeEx(string s)
        {
            string replace = @"\u" + Regex.Replace(s, @"(\w{4})(?=[^\s])", @"$1\u");
            return Decode(replace);
        }
    }
}

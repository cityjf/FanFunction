using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;

namespace FanFunction
{
    /// <summary>
    /// 关于字符串等操作的类
    /// </summary>
    public class clsStr
    {
        /// <summary>
        /// 字符串首字母转换成大写
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string ToStringFirstLetterToUpper(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            else
                return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
        /// <summary>
        /// 字符串首字母转换成小写
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns></returns>
        public static string ToStringFirstLetterToLower(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            else
                return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
        /// <summary>
        /// 截取字符串从0开始，可以用于截取路径部分的除文件名外的部分
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="strLastIndexOf">根据什么截取</param>
        /// <returns></returns>
        public static string SubstringBegin(string str, string strLastIndexOf)
        {
            return str = str.Substring(0, str.LastIndexOf(strLastIndexOf) + 1);
        }
        /// <summary>
        /// 截取分隔符之后的字符串,可以用于截取路径后面的文件名
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="strLastIndexOf">根据什么截取</param>
        /// <returns></returns>
        public static string SubstringEnd(string str, string strLastIndexOf)
        {
            return str = str.Substring(str.LastIndexOf(strLastIndexOf) + 1);
        }
        /// <summary>
        /// 根据文件的大小获取文件的单位（括号内未实现：按照windows的算法，小于10的保留2位小数，大于等于10小于100的保留一位小数，大于等于100的不保留小数）
        /// </summary>
        /// <param name="fileSize">文件大小</param>
        /// <param name="unit">单位描述</param>
        /// <returns></returns>
        public static double GetUnitByFileSize(int fileSize, ref string unit)
        {
            double size = fileSize;
            if (fileSize < 1024)
            {
                unit = "字节";
            }
            else
            {
                size = size / 1024;
                if (size >= 1 && size < 1024)
                {
                    unit = "KB";
                }
                else
                {
                    size = size / 1024;
                    if (size >= 1 && size < 1024)
                    {
                        unit = "MB";
                    }
                    else
                    {
                        size = size / 1024;
                        unit = "GB";
                    }
                }
            }
            return size;
        }
        /// <summary>
        /// 清除html标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring)
        {
            return Regex.Replace(Htmlstring, "<[^>]*>", "").Trim();
        }
        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="input">要转换的字符串</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 全角转半角的函数
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static char ToDBC(char input)
        {
            if (input == 12288)
            {
                input = (char)32;
            }
            if (input > 65280 && input < 65375)
                input = (char)(input - 65248);
            return input;
        }
        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="input">要转换的字符串</param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 半角转全角的函数
        /// 全角空格为12288，半角空格为32
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static char ToSBC(char input)
        {
            if (input == 32)
            {
                input = (char)12288;
            }
            if (input < 127)
                input = (char)(input + 65248);
            return input;
        }
        /// <summary>
        /// 去掉字符串中的连续空格只保留一个空格，如" a     b c "转换以后是"a b c"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string KeepOneSpace(string str)
        {
            Regex replaceSpace = new Regex(@"\s{1,}", RegexOptions.IgnoreCase);
            return replaceSpace.Replace(str, " ").Trim();
        }
    }
}

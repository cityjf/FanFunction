using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 字符串常用扩展方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns>如果 value 参数为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        /// <summary>
        /// 转换成string
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="str">当转换失败时使用的字符串</param>
        /// <returns></returns>
        public static string ToString(this object obj, string str = "")
        {
            if (obj == null) return str;
            else return obj.ToString();
        }
        /// <summary>
        /// 根据字符串分隔字符串
        /// </summary>
        /// <param name="input">要拆分的字符串</param>
        /// <param name="separator">分隔此字符串的子字符串</param>
        /// <returns></returns>
        public static List<string> Split(this string input, string separator)
        {
            return Regex.Split(input, separator, RegexOptions.IgnoreCase).ToList();
        }
        /// <summary>
        /// 根据字符串分隔字符串
        /// </summary>
        /// <param name="input">要拆分的字符串</param>
        /// <param name="separator">分隔此字符串的子字符串</param>
        /// <param name="options">匹配选项</param>
        /// <returns></returns>
        public static List<string> Split(this string input, string separator, RegexOptions options)
        {
            return Regex.Split(input, separator, options).ToList();
        }
        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项。
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns></returns>
        public static string Match(this string input, string pattern)
        {
            return Regex.Match(input, pattern).Value;
        }
        /// <summary>
        /// 转换字符串集合为字符串，用于sql中的in语句中
        /// </summary>
        /// <param name="arrID"></param>
        /// <returns></returns>
        public static string GetSqlStrByList(this List<string> arrID)
        {
            return string.Format("'{0}'", string.Join("','", arrID));
        }
        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。指定的参数提供区域性特定的格式设置信息。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns>format 的副本，其中的格式项已替换为 args 中相应对象的字符串表示形式。</returns>
        public static string FormatWith(this string target, params object[] args)
        {
            return string.Format(target, args);
        }

        #region String转换成其他类型的方法
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns></returns>
        public static bool IsInt(this string value)
        {
            int number;
            return int.TryParse(value, out number);
        }
        /// <summary>
        /// 转换成int,如果失败则返回0
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            int number;
            int.TryParse(value, out number);
            return number;
        }
        /// <summary>
        /// 判断是否为long
        /// </summary>
        /// <param name="value">要判断的对象</param>
        /// <returns></returns>
        public static bool IsLong(this string value)
        {
            long number;
            return long.TryParse(value, out number);
        }
        /// <summary>
        /// 转换成long,如果失败则返回0
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <returns></returns>
        public static long ToLong(this string value)
        {
            long number;
            long.TryParse(value, out number);
            return number;
        }
        /// <summary>
        /// 判断是否为short
        /// </summary>
        /// <param name="value">要判断的对象</param>
        /// <returns></returns>
        public static bool IsShort(this string value)
        {
            short number;
            return short.TryParse(value, out number);
        }
        /// <summary>
        /// 转换成short,如果失败则返回0
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <returns></returns>
        public static short ToShort(this string value)
        {
            short number;
            short.TryParse(value, out number);
            return number;
        }
        /// <summary>
        /// 判断是否为DateTime
        /// </summary>
        /// <param name="value">要判断的对象</param>
        /// <returns></returns>
        public static bool IsDateTime(this string value)
        {
            DateTime dt;
            return DateTime.TryParse(value, out dt);
        }
        /// <summary>
        /// 转换成DateTime格式,如果失败则返回最小时间(0001/1/1 0:00:00)
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(value, out dt);
            return dt;
        }
        /// <summary>
        /// 判断是否为bool
        /// </summary>
        /// <param name="value">要判断的对象</param>
        /// <returns></returns>
        public static bool IsBool(this string value)
        {
            bool b;
            return bool.TryParse(value, out b);
        }
        /// <summary>
        /// 转换成bool,失败则返回参数指定的值，如果不指定参数转换失败则返回false
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <param name="defaultValue">转换失败时返回的值</param>
        /// <returns></returns>
        public static bool ToBool(this string value, bool defaultValue = false)
        {
            bool b;
            if (!bool.TryParse(value, out b))
                return defaultValue;
            return b;
        }
        /// <summary>
        /// 返回一个值，该值指示指定的 System.String 对象是否出现在此字符串中，是否忽略大小写
        /// </summary>
        /// <param name="value">要搜寻的字符串</param>
        /// <param name="ignoreCase">要在比较过程中忽略大小写，则为 true；否则为 false。</param>
        /// <returns>返回一个值，该值指示指定的 System.String 对象是否出现在此字符串中，是否忽略大小写</returns>
        public static bool Contains(this string target, string value, bool ignoreCase)
        {
            if (ignoreCase)
                return target.ToLower().Contains(value.ToLower());
            else
                return target.Contains(value);
        }
        #endregion
    }
}

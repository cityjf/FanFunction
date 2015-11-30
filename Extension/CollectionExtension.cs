using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace System
{
    /// <summary>
    /// Collection扩展方法
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 是否为空或null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
        /// <summary>
        /// 转换字符串数组,如果返回null则是转换失败
        /// </summary>
        /// <typeparam name="T">可以是Guid、int、DateTime、Decimal、short、double、long、bool</typeparam>
        /// <param name="collection"></param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(this ICollection<string> collection)
        {
            Type type = typeof(T);
            try
            {
                if (type == typeof(Guid))
                {
                    return collection.ToList().ConvertAll((i) => Guid.Parse(i)) as List<T>;
                }
                else if (type == typeof(int))
                {
                    return collection.ToList().ConvertAll((i) => int.Parse(i)) as List<T>;
                }
                else if (type == typeof(DateTime))
                {
                    return collection.ToList().ConvertAll((i) => DateTime.Parse(i)) as List<T>;
                }
                else if (type == typeof(Decimal))
                {
                    return collection.ToList().ConvertAll((i) => Decimal.Parse(i)) as List<T>;
                }
                else if (type == typeof(short))
                {
                    return collection.ToList().ConvertAll((i) => short.Parse(i)) as List<T>;
                }
                else if (type == typeof(double))
                {
                    return collection.ToList().ConvertAll((i) => double.Parse(i)) as List<T>;
                }
                else if (type == typeof(long))
                {
                    return collection.ToList().ConvertAll((i) => long.Parse(i)) as List<T>;
                }
                else if (type == typeof(bool))
                {
                    return collection.ToList().ConvertAll((i) => bool.Parse(i)) as List<T>;
                }
                else if (type == typeof(TimeSpan))
                {
                    return collection.ToList().ConvertAll((i) => TimeSpan.Parse(i)) as List<T>;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 确定某元素是否在 System.Collections.Generic.List&lt;string&gt; 中。是否忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value">要搜寻的字符串</param>
        /// <param name="ignoreCase">要在比较过程中忽略大小写，则为 true；否则为 false。</param>
        /// <returns>返回一个值，该值指示指定的 System.String 对象是否出现在此集合中，是否忽略大小写</returns>
        public static bool Contains(this List<string> source, string value, bool ignoreCase)
        {
            if (ignoreCase)
                return source.Exists(w => w.ToLower() == value.ToLower());
            else
                return source.Contains(value);
        }
    }
}

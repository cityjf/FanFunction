using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System
{
    public static class EntityExtension
    {
        /// <summary>
        /// 从当前对象移除所有String类型属性的空白字符和尾部空白字符。
        /// </summary>
        public static void Trim<T>(this T t)
        {
            Type type = typeof(T);
            PropertyInfo[] fields = type.GetProperties();//获取指定对象的所有公共属性
            foreach (PropertyInfo p in fields)
            {
                //当属性只写的时候跳过
                if (!p.CanRead) continue;
                //获取当前属性值
                var value = p.GetValue(t, null);
                //如果当前的属性是String类型，并且值不为null，并且可写，则去掉空格后重新赋值
                if (p.PropertyType == typeof(String) && value != null && p.CanWrite)
                {
                    p.SetValue(t, value.ToString().Trim(), null);
                }
            }
        }
    }
}

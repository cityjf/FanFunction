using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanFunction
{
    /// <summary>
    /// 用于设置表的中文名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string chsName)
        {
            tableChsName = chsName;
        }

        private string tableChsName;
        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableChsName
        {
            get { return tableChsName; }
            set { tableChsName = value; }
        }
        /// <summary>
        /// 获取表中文名
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>表中文名</returns>
        public static string GetTableChsName<T>()
        {
            string tableChsName = string.Empty;
            try
            {
                object[] objAttrs = typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
                if (objAttrs.Length > 0)
                {
                    tableChsName = (objAttrs[0] as TableAttribute).TableChsName;
                }
            }
            catch { }
            return tableChsName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanFunction
{
    /// <summary>
    /// 用于设置字段的中文名、字段类型、字段长度、是否可空、是否主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 带参的构造函数
        /// </summary>
        /// <param name="strChsName">中文名</param>
        public FieldAttribute(string strChsName)
        {
            chsName = strChsName;
        }
        private string chsName;
        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string ChsName
        {
            get { return chsName; }
            set { chsName = value; }
        }
        private string fieldType;
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType
        {
            get { return fieldType; }
            set { fieldType = value; }
        }
        private string fieldLength;
        /// <summary>
        /// 字段长度
        /// </summary>
        public string FieldLength
        {
            get { return fieldLength; }
            set { fieldLength = value; }
        }
        private bool isNull = true;
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNull
        {
            get { return isNull; }
            set { isNull = value; }
        }

        private bool isPrimaryKey = false;
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return isPrimaryKey; }
            set { isPrimaryKey = value; }
        }

        /// <summary>
        /// 获取字段中文名
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>字段中文名</returns>
        public static string GetFieldChsName<T>(string fieldName)
        {
            string fieldChsName = string.Empty;
            try
            {
                object[] objAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(FieldAttribute), false);
                if (objAttrs.Length > 0)
                {
                    fieldChsName = (objAttrs[0] as FieldAttribute).ChsName;
                }
            }
            catch { }
            return fieldChsName;
        }
        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>字段类型</returns>
        public static string GetFieldType<T>(string fieldName)
        {
            string fieldType = string.Empty;
            try
            {
                object[] objAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(FieldAttribute), false);
                if (objAttrs.Length > 0)
                {
                    fieldType = (objAttrs[0] as FieldAttribute).FieldType;
                }
            }
            catch { }
            return fieldType;
        }
        /// <summary>
        /// 获取字段长度
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>字段长度</returns>
        public static string GetFieldLength<T>(string fieldName)
        {
            string fieldLength = string.Empty;
            try
            {
                object[] objAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(FieldAttribute), false);
                if (objAttrs.Length > 0)
                {
                    fieldLength = (objAttrs[0] as FieldAttribute).FieldLength;
                }
            }
            catch { }
            return fieldLength;
        }
        /// <summary>
        /// 获取字段是否可空
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>字段是否可空</returns>
        public static bool GetIsNull<T>(string fieldName)
        {
            bool isNull = true;
            try
            {
                object[] objAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(FieldAttribute), false);
                if (objAttrs.Length > 0)
                {
                    isNull = (objAttrs[0] as FieldAttribute).IsNull;
                }
            }
            catch { }
            return isNull;
        }
        /// <summary>
        /// 获取字段是否主键
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>字段是否主键</returns>
        public static bool GetIsPrimaryKey<T>(string fieldName)
        {
            bool isPrimaryKey = false;
            try
            {
                object[] objAttrs = typeof(T).GetProperty(fieldName).GetCustomAttributes(typeof(FieldAttribute), false);
                if (objAttrs.Length > 0)
                {
                    isPrimaryKey = (objAttrs[0] as FieldAttribute).IsPrimaryKey;
                }
            }
            catch { }
            return isPrimaryKey;
        }
    }
}

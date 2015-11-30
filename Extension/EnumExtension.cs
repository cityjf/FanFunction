using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;

namespace System
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举 属性上的 Description描述
        /// </summary>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string GetDescription(this object enumObj)
        {
            if (enumObj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = enumObj.GetType();
                //DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, enumObj));
                DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }

            return enumObj.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;

namespace FanFunction
{
    #region DataTable操作相关类
    /// <summary>
    /// DataTable操作相关类
    /// </summary>
    public static class clsDataTable
    {
        /// <summary>
        /// 去掉DataTable中的重复行
        /// </summary>
        /// <param name="objDT"></param>
        /// <returns></returns>
        public static DataTable RemoveRepeatRows(this DataTable objDT)
        {
            //for (int i = 0; i < objDT.Rows.Count; i++)
            //{
            //    for (int j = 0; j < objDT.Rows.Count; j++)
            //    {
            //        if (objDT.Rows[i].Equals(objDT.Rows[j]))
            //        {
            //            objDT.Rows.RemoveAt(i);
            //        }
            //    }
            //}
            //return objDT;
            DataView dv = new DataView(objDT);
            string[] columnName = new string[objDT.Columns.Count];
            for (int i = 0; i < objDT.Columns.Count; i++)
            {
                columnName[i] = objDT.Columns[i].ColumnName;
            }
            return dv.ToTable(true, columnName);
        }
        /// <summary>
        /// 复制一个DataTable(包括数据、结构)
        /// </summary>
        /// <param name="objDT"></param>
        /// <returns></returns>
        public static DataTable DataTableCopy(this DataTable objDT)
        {
            //DataTable dt = new DataTable();
            //dt = objDT.Clone();//把objDT得结构传给objDT
            //foreach (DataRow objRow in objDT.Rows)
            //{
            //    dt.ImportRow(objRow);//循环复制
            //}
            return objDT.Copy();
        }
        /// <summary>
        /// 将DataTable转换成指定List集合
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dt">需要转换的DataTable</param>
        /// <returns>对象集合</returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (DataRow dr in dt.Rows)
            {
                object obj = Activator.CreateInstance(t, null);//创建指定类型实例
                foreach (PropertyInfo p in fields)
                {
                    if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                    {
                        p.SetValue(obj, Convert.ChangeType(dr[p.Name], p.PropertyType), null);//给对象赋值
                    }
                }
                list.Add((T)obj);//将对象填充到list集合
            }
            return list;
        }
        /// <summary>
        /// List集合转DataTable
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="list">List集合</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            Type type = typeof(T);
            DataTable dt = new DataTable(type.Name);
            PropertyInfo[] fields = type.GetProperties();//获取指定对象的所有公共属性
            //设置列
            foreach (PropertyInfo p in fields)
            {
                Type tt = p.PropertyType;
                //转换Nullable类型
                if (tt.IsGenericType && (tt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    tt = tt.GetGenericArguments()[0];
                }
                dt.Columns.Add(p.Name, tt);
            }
            //复制数据
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < fields.Length; i++)
                {
                    row[i] = fields[i].GetValue(t, null) == null ? DBNull.Value : fields[i].GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
    #endregion
}

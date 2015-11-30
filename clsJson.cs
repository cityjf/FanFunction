using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;

namespace FanFunction
{
    /// <summary>
    /// framwork 3.5以上的版本才有的，需要添加对System.Web.Extensions.dll的引用
    /// </summary>
    public class clsJson
    {
        /// <summary>
        /// 对象或对象列表转换成Json字符串
        /// </summary>
        /// <param name="obj">对象或对象集合</param>
        /// <returns>Json字符串</returns>
        public static string ObjectToJson(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }
        /// <summary>
        /// 将DataTable转换成Json字符串
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Json字符串</returns>
        public static string DataTableToJson(DataTable dt)
        {
            return new JavaScriptSerializer().Serialize(DataTableToList(dt));
        }
        /// <summary>
        /// Json转换成对象或对象集合
        /// </summary>
        /// <typeparam name="T">T或者List&lt;T&gt;</typeparam>
        /// <param name="jsonText">json串</param>
        /// <returns>对象或对象集合</returns>
        public static T JsonToObject<T>(string jsonText)
        {
            return new JavaScriptSerializer().Deserialize<T>(jsonText);
        }
        /// <summary>
        /// DataTable转换成Dictionary键值对集合
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>键值对集合</returns>
        private static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }
        /// <summary>
        /// 转换为EsayUI特定的json格式
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="total">总记录数</param>
        /// <returns>json串</returns>
        public static string EsayUI_Json(object obj, int total)
        {
            return "{\"total\":" + total + ",\"rows\":" + ObjectToJson(obj) + "}";
        }
    }
}

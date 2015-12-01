using System;
using System.Collections.Generic;
using System.Text;

namespace FanFunction
{
    /// <summary>
    /// App.config或web.config相关函数
    /// </summary>
    public class clsConfig
    {
        /// <summary>
        /// 获取App.config或web.config中add key对应的value值
        /// </summary>
        /// <param name="key">key的name</param>
        /// <returns>value</returns>
        public static string GetKeyValueByKey(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 获取App.config或web.config中connectionStrings的name对应的值
        /// </summary>
        /// <param name="strConnName">connectionStrings的name</param>
        /// <returns>value</returns>
        public static string GetConnectionString(string strConnName)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[strConnName].ConnectionString;
        }
        /// <summary>
        /// 获取name为ConnectionString的连接串
        /// </summary>
        /// <returns>value</returns>
        public static string GetConnectionString()
        {
            return GetConnectionString("ConnectionString");
        }
        /// <summary>
        /// 获取App.config或web.config中connectionStrings的name对应的SqlConnection
        /// </summary>
        /// <param name="strConnName">connectionStrings的name</param>
        /// <returns>SqlConnection</returns>
        public static System.Data.SqlClient.SqlConnection GetSqlConn(string strConnName)
        {
            return new System.Data.SqlClient.SqlConnection(GetConnectionString(strConnName));
        }
    }
}

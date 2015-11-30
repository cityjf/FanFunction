using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using FanFunction.db;
using System.Data;

namespace FanFunction
{
    /// <summary>
    /// 关于数据库操作的类
    /// </summary>
    public class clsDatabase
    {
        #region 备份数据库
        /// <summary>
        /// 备份数据库的方法
        /// </summary>
        /// <param name="path">备份文件的完整路径</param>
        /// <param name="DBName">数据库名称</param>
        /// <param name="conn">连接串</param>
        /// <returns></returns>
        public static bool BackupDB(string path, string DBName, SqlConnection conn)
        {
            //文件夹不存在就创建
            if (Directory.Exists(clsStr.SubstringBegin(path, "\\")) == false)
            {
                Directory.CreateDirectory(clsStr.SubstringBegin(path, "\\"));
            }
            string strSQL = string.Format("BACKUP DATABASE {0} TO DISK='{1}'", DBName, path);
            return DBHelper.ExecuteCommand(strSQL, conn) > 0;
        }
        /// <summary>
        /// 备份数据库的方法
        /// </summary>
        /// <param name="path">备份文件的完整路径</param>
        /// <param name="DBName">数据库名称</param>
        /// <returns></returns>
        public static bool BackupDB(string path, string DBName)
        {
            return BackupDB(path, DBName, DBHelper.Connection);
        }
        #endregion

        #region 获取数据库最大编号+1
        /// <summary>
        /// 获取数据库最大编号+1
        /// </summary>
        /// <param name="strTabName">表名</param>
        /// <param name="strFieldName">字段名</param>
        /// <param name="strWhere">where条件</param>
        /// <returns>最大编号+1</returns>
        public static string GetMaxStrId(string strTabName, string strFieldName, string strWhere)
        {
            string strSQL = "select Max(" + strFieldName + ") from " + strTabName + " where " + strWhere;
            string strMax = DBHelper.GetScalar(strSQL).ToString();
            int iLastNum = int.Parse(strMax.Substring(strMax.Length - 1));//最后一位的数字
            strMax = strMax.Substring(0, strMax.Length - 1) + (iLastNum + 1).ToString();
            return strMax;
        }
        /// <summary>
        /// 获取表主键的最大编号+1
        /// </summary>
        /// <param name="strTabName">表名</param>
        /// <param name="strFieldName">字段名</param>
        /// <returns>最大编号+1</returns>
        public static string GetMaxStrId(string strTabName, string strFieldName)
        {
            return GetMaxStrId(strTabName, strFieldName, "1=1");
        }
        #endregion

        #region 查询数据库所有表或视图
        /// <summary>
        /// 查询数据库所有表或视图
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="xtype">1查询表,2查询视图,0查询表和视图</param>
        /// <returns></returns>
        public static List<string> GetAllTable(SqlConnection conn, int xtype = 1)
        {
            string strSQL = string.Format("select name from [{0}] ..sysobjects", conn.Database);
            if (xtype == 1)
                strSQL += " where xtype='u'";
            else if (xtype == 2)
                strSQL += " where xtype='v'";
            else if (xtype == 0)
                strSQL += " where xtype in ('u','v')";
            DataTable dt = DBHelper.GetDataTable(strSQL, conn);
            List<string> names = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                names.Add(row[0].ToString());
            }
            return names;
        }
        #endregion

        #region 获取数据库表的所有字段信息
        /// <summary>
        /// 获取数据库某表的所有字段[表名:tableName,字段索引:fieldIndex,字段名:fieldName,字段类型:fieldType,字段长度:fieldLength,是否为空:fieldIsNull,字段说明:fieldMemo]
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<FieldInfo> GetTableFields(SqlConnection conn, string tableName)
        {
            string strSQL = string.Format("SELECT sys.sysobjects.name AS tableName, sys.syscolumns.colid AS fieldIndex, sys.syscolumns.name AS fieldName, sys.systypes.name AS fieldType, sys.syscolumns.length AS fieldLength, CASE syscolumns.isnullable WHEN '0' THEN 'false' ELSE 'true' END AS fieldIsNull, sys.extended_properties.value AS fieldMemo FROM sys.sysobjects INNER JOIN sys.syscolumns ON sys.sysobjects.id = sys.syscolumns.id LEFT OUTER JOIN sys.systypes ON sys.syscolumns.xtype = sys.systypes.xusertype LEFT OUTER JOIN sys.extended_properties ON sys.syscolumns.id = sys.extended_properties.major_id AND sys.syscolumns.colid = sys.extended_properties.minor_id where (sys.sysobjects.name = '{0}')", tableName);
            return DBHelper.GetDataTable(strSQL, conn).ToList<FieldInfo>();
        }
        #endregion

    }
    #region 字段实体
    /// <summary>
    /// 字段实体
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 字段索引
        /// </summary>
        public int FieldIndex { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int FieldLength { get; set; }
        /// <summary>
        /// 是否为空
        /// </summary>
        public bool FieldIsNull { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        public string FieldMemo { get; set; }
    }
    #endregion
}

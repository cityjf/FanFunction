using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Linq;

namespace FanFunction.db
{
    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public abstract class DBHelper
    {
        #region SqlConnection相关
        private static SqlConnection connection;
        /// <summary>    
        /// 数据库连接，默认是连接串的名称为ConnectionString
        /// </summary>    
        public static SqlConnection Connection
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                if (connection == null)
                {
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                else if (connection.State == System.Data.ConnectionState.Broken)
                {
                    connection.Close();
                    connection.Open();
                }
                return connection;
            }
        }
        /// <summary>
        /// 根据配置文件中的链接串名称获取SqlConnection
        /// </summary>
        /// <param name="strConnectionStringName">连接串名称</param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string strConnectionStringName)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[strConnectionStringName].ConnectionString);
        }
        /// <summary>
        /// 根据配置文件中的链接串名称获取ConnectionString
        /// </summary>
        /// <param name="strConnectionStringName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string strConnectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[strConnectionStringName].ConnectionString;
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="conn"></param>
        public static void OpenConn(SqlConnection conn)
        {
            if (conn == null)
            {
                throw new Exception("当前连接为空");
            }
            else if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            else if (conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="conn"></param>
        public static void CloseConn(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
            else if (conn.State == System.Data.ConnectionState.Broken)
            {
                conn.Close();
            }
        }
        #endregion
        #region 执行SQL
        /// <summary>    
        /// 执行sql语句，返回受影响的行数
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <returns>返回受影响的行数</returns>    
        public static int ExecuteCommand(string strSQL)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行sql语句，返回受影响的行数（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="strSQL">sql语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteCommand(string strSQL, SqlConnection newConn)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 参数化执行sql语句，返回受影响的行数
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回受影响的行数</returns>    
        public static int ExecuteCommand(string strSQL, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteNonQuery();
        }
        /// <summary>    
        /// 参数化执行sql语句，返回受影响的行数（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回受影响的行数</returns>    
        public static int ExecuteCommand(string strSQL, SqlConnection newConn, params SqlParameter[] values)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                cmd.Parameters.AddRange(values);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 执行sql语句，返回第一行第一列的值
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <returns>返回第一行第一列的值</returns>    
        public static object GetScalar(string strSQL)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            return cmd.ExecuteScalar();
        }
        /// <summary>    
        /// 执行sql语句，返回第一行第一列的值（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>返回第一行第一列的值</returns>    
        public static object GetScalar(string strSQL, SqlConnection newConn)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 参数化查询，返回第一行第一列的值  
        /// </summary>    
        /// <param name="strSQL">sql语句</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回第一行第一列的值</returns>    
        public static object GetScalar(string strSQL, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteScalar();
        }
        /// <summary>    
        /// 参数化查询，返回第一行第一列的值（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="strSQL">sql语句</param>   
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回第一行第一列的值</returns>    
        public static object GetScalar(string strSQL, SqlConnection newConn, params SqlParameter[] values)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                cmd.Parameters.AddRange(values);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 执行sql查询语句，返回sqlDataReader
        /// </summary>    
        /// <param name="strSQL">sql查询语句</param>    
        /// <returns>返回sqlDataReader</returns>    
        public static SqlDataReader GetReader(string strSQL)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            return cmd.ExecuteReader();
        }
        /// <summary>    
        /// 执行sql查询语句，返回sqlDataReader（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="strSQL">sql查询语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>返回sqlDataReader</returns>    
        public static SqlDataReader GetReader(string strSQL, SqlConnection newConn)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 参数化查询，返回sqlDataReader    
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回sqlDataReader</returns>    
        public static SqlDataReader GetReader(string strSQL, params SqlParameter[] values)
        {
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            cmd.Parameters.AddRange(values);
            return cmd.ExecuteReader();
        }
        /// <summary>    
        /// 参数化查询，返回sqlDataReader（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="strSQL">sql语句</param>    
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回sqlDataReader</returns>    
        public static SqlDataReader GetReader(string strSQL, SqlConnection newConn, params SqlParameter[] values)
        {
            try
            {
                OpenConn(newConn);
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                cmd.Parameters.AddRange(values);
                return cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 执行sql查询语句，返回DataTable
        /// </summary>    
        /// <param name="strSQL">sql查询语句</param>    
        /// <returns>返回DataTable</returns>    
        public static DataTable GetDataTable(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 执行sql查询语句，返回DataTable（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="strSQL">sql查询语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>返回DataTable</returns>
        public static DataTable GetDataTable(string strSQL, SqlConnection newConn)
        {
            try
            {
                OpenConn(newConn);
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 参数化查询，返回DataTable
        /// </summary>    
        /// <param name="sql">sql查询语句</param>    
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回DataTable</returns>    
        public static DataTable GetDataTable(string strSQL, params SqlParameter[] values)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            cmd.Parameters.AddRange(values);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        /// <summary>    
        /// 参数化查询，返回DataTable（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="sql">sql查询语句</param>    
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回DataTable</returns>    
        public static DataTable GetDataTable(string strSQL, SqlConnection newConn, params SqlParameter[] values)
        {
            try
            {
                OpenConn(newConn);
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                cmd.Parameters.AddRange(values);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 执行sql查询语句，返回DataSet
        /// </summary>    
        /// <param name="strSQL">sql查询语句</param>    
        /// <returns>返回DataSet</returns>    
        public static DataSet GetDataSet(string strSQL)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 执行sql查询语句，返回DataSet（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="strSQL">sql查询语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>返回DataSet</returns>
        public static DataSet GetDataSet(string strSQL, SqlConnection newConn)
        {
            try
            {
                OpenConn(newConn);
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>    
        /// 参数化查询，返回DataSet
        /// </summary>    
        /// <param name="sql">sql查询语句</param>    
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回DataSet</returns>    
        public static DataSet GetDataSet(string strSQL, params SqlParameter[] values)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(strSQL, Connection);
            cmd.Parameters.AddRange(values);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        /// <summary>    
        /// 参数化查询，返回DataSet（指定数据库连接串，适用于跨数据库操作）
        /// </summary>    
        /// <param name="sql">sql查询语句</param>    
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>返回DataSet</returns>    
        public static DataSet GetDataSet(string strSQL, SqlConnection newConn, params SqlParameter[] values)
        {
            try
            {
                OpenConn(newConn);
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand(strSQL, newConn);
                cmd.Parameters.AddRange(values);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConn(newConn);
            }
        }
        /// <summary>
        /// 是否存在满足条件的记录，存在返回True不存在返回False
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strCondition">where后面的查询条件</param>
        /// <returns>存在返回True不存在返回False</returns>
        public static bool IsExist(string tableName, string strCondition)
        {
            string strSQL = "select count(*) from " + tableName + " where " + strCondition;
            try
            {
                if (GetScalar(strSQL).ToString() == "0") return false;
                else return true;
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        /// <summary>
        /// 是否存在满足条件的记录，存在返回True不存在返回False（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strCondition">where后面的查询条件</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>存在返回True不存在返回False</returns>
        public static bool IsExist(string tableName, string strCondition, SqlConnection newConn)
        {
            string strSQL = "select count(*) from " + tableName + " where " + strCondition;
            try
            {
                if (GetScalar(strSQL, newConn).ToString() == "0") return false;
                else return true;
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        /// <summary>
        /// 参数化查询是否存在满足条件的记录，存在返回True不存在返回False
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strCondition">where后面的查询条件</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>存在返回True不存在返回False</returns>
        public static bool IsExist(string tableName, string strCondition, params SqlParameter[] values)
        {
            string strSQL = "select count(*) from " + tableName + " where " + strCondition;
            try
            {
                if (GetScalar(strSQL, values).ToString() == "0") return false;
                else return true;
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        /// <summary>
        /// 参数化查询是否存在满足条件的记录，存在返回True不存在返回False（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strCondition">where后面的查询条件</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="values">SqlParameter数组</param>    
        /// <returns>存在返回True不存在返回False</returns>
        public static bool IsExist(string tableName, string strCondition, SqlConnection newConn, params SqlParameter[] values)
        {
            string strSQL = "select count(*) from " + tableName + " where " + strCondition;
            try
            {
                if (GetScalar(strSQL, newConn, values).ToString() == "0") return false;
                else return true;
            }
            catch (Exception objException)
            {
                throw objException;
            }
        }
        #endregion
        #region 分页查询、增、删、改、转换一系列方法(反射)
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">orderBy后面的语句</param>
        /// <param name="columns">查询的列</param>
        /// <param name="condition">条件</param>
        /// <param name="total">查询的总记录</param>
        /// <returns>DataTable</returns>
        public static DataTable Search_DataTable(string tableName, string orderBy, out int total, int pageIndex = 1, int pageSize = 10, string columns = "*", string condition = "")
        {
            SqlCommand cmd = new SqlCommand("p_Pager", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int, 4) { Value = pageSize });
            cmd.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int, 4) { Value = pageIndex });
            cmd.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar, 100) { Value = tableName });
            cmd.Parameters.Add(new SqlParameter("@orderColumn", SqlDbType.NVarChar, 100) { Value = orderBy });
            cmd.Parameters.Add(new SqlParameter("@columns", SqlDbType.NVarChar, 1000) { Value = columns });
            cmd.Parameters.Add(new SqlParameter("@condition", SqlDbType.NVarChar, 1000) { Value = condition });
            cmd.Parameters.Add(new SqlParameter("@totalCount", SqlDbType.Int, 4));
            cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            total = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
            return dt;
        }
        /// <summary>
        /// 分页查询（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">orderBy后面的语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="columns">查询的列</param>
        /// <param name="condition">条件</param>
        /// <param name="total">查询的总记录</param>
        /// <returns>DataTable</returns>
        public static DataTable Search_DataTable(string tableName, string orderBy, SqlConnection newConn, out int total, int pageIndex = 1, int pageSize = 10, string columns = "*", string condition = "")
        {
            OpenConn(newConn);
            SqlCommand cmd = new SqlCommand("p_Pager", newConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int, 4) { Value = pageSize });
            cmd.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int, 4) { Value = pageIndex });
            cmd.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar, 100) { Value = tableName });
            cmd.Parameters.Add(new SqlParameter("@orderColumn", SqlDbType.NVarChar, 100) { Value = orderBy });
            cmd.Parameters.Add(new SqlParameter("@columns", SqlDbType.NVarChar, 1000) { Value = columns });
            cmd.Parameters.Add(new SqlParameter("@condition", SqlDbType.NVarChar, 1000) { Value = condition });
            cmd.Parameters.Add(new SqlParameter("@totalCount", SqlDbType.Int, 4));
            cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            total = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
            CloseConn(newConn);
            return dt;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">orderBy后面的语句</param>
        /// <param name="columns">查询的列</param>
        /// <param name="condition">条件</param>
        /// <param name="total">查询的总记录</param>
        /// <returns>实体集合</returns>
        public static List<T> Search<T>(string tableName, string orderBy, out int total, int pageIndex = 1, int pageSize = 10, string columns = "*", string condition = "")
        {
            //执行存储过程查询出SqlDataReader
            SqlCommand cmd = new SqlCommand("p_Pager", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int, 4) { Value = pageSize });
            cmd.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int, 4) { Value = pageIndex });
            cmd.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar, 100) { Value = tableName });
            cmd.Parameters.Add(new SqlParameter("@orderColumn", SqlDbType.NVarChar, 100) { Value = orderBy });
            cmd.Parameters.Add(new SqlParameter("@columns", SqlDbType.NVarChar, 1000) { Value = columns });
            cmd.Parameters.Add(new SqlParameter("@condition", SqlDbType.NVarChar, 1000) { Value = condition });
            cmd.Parameters.Add(new SqlParameter("@totalCount", SqlDbType.Int, 4));
            cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
            SqlDataReader reader = cmd.ExecuteReader();
            //SqlDataReader转换成List<T>
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] fields = t.GetProperties();
            while (reader.Read())
            {
                object obj = Activator.CreateInstance(t, null);
                foreach (System.Reflection.PropertyInfo p in fields)
                {
                    if (!Convert.IsDBNull(reader[p.Name]))
                    {
                        p.SetValue(obj, reader[p.Name], null);
                    }
                }
                list.Add((T)obj);
            }
            reader.Close();
            total = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
            return list;
        }
        /// <summary>
        /// 分页查询（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="tableName">表名</param>
        /// <param name="orderBy">orderBy后面的语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="columns">查询的列</param>
        /// <param name="condition">条件</param>
        /// <param name="total">查询的总记录</param>
        /// <returns>实体集合</returns>
        public static List<T> Search<T>(string tableName, string orderBy, SqlConnection newConn, out int total, int pageIndex = 1, int pageSize = 10, string columns = "*", string condition = "")
        {
            //执行存储过程查询出SqlDataReader
            OpenConn(newConn);
            SqlCommand cmd = new SqlCommand("p_Pager", newConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.Int, 4) { Value = pageSize });
            cmd.Parameters.Add(new SqlParameter("@pageIndex", SqlDbType.Int, 4) { Value = pageIndex });
            cmd.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar, 100) { Value = tableName });
            cmd.Parameters.Add(new SqlParameter("@orderColumn", SqlDbType.NVarChar, 100) { Value = orderBy });
            cmd.Parameters.Add(new SqlParameter("@columns", SqlDbType.NVarChar, 1000) { Value = columns });
            cmd.Parameters.Add(new SqlParameter("@condition", SqlDbType.NVarChar, 1000) { Value = condition });
            cmd.Parameters.Add(new SqlParameter("@totalCount", SqlDbType.Int, 4));
            cmd.Parameters["@totalCount"].Direction = ParameterDirection.Output;
            SqlDataReader reader = cmd.ExecuteReader();
            //SqlDataReader转换成List<T>
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] fields = t.GetProperties();
            while (reader.Read())
            {
                object obj = Activator.CreateInstance(t, null);
                foreach (System.Reflection.PropertyInfo p in fields)
                {
                    if (!Convert.IsDBNull(reader[p.Name]))
                    {
                        p.SetValue(obj, reader[p.Name], null);
                    }
                }
                list.Add((T)obj);
            }
            reader.Close();
            total = int.Parse(cmd.Parameters["@totalCount"].Value.ToString());
            CloseConn(newConn);
            return list;
        }
        /// <summary>
        /// 将DataTable转换成指定List集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">需要转换的DataTable</param>
        /// <returns>对象集合</returns>
        public static List<T> ToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (DataRow dr in dt.Rows)
            {
                T obj = Activator.CreateInstance<T>();//创建指定类型实例
                foreach (System.Reflection.PropertyInfo p in fields)
                {
                    if (dt.Columns.Contains(p.Name) && !Convert.IsDBNull(dr[p.Name]))
                    {
                        p.SetValue(obj, Convert.ChangeType(dr[p.Name], p.PropertyType), null);//给对象赋值
                    }
                }
                list.Add(obj);//将对象填充到list集合
            }
            return list;
        }
        /// <summary>
        /// 将SqlDataReader转换成指定List集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="reader">需要转换的SqlDataReader</param>
        /// <returns>对象集合</returns>
        public static List<T> ToList<T>(SqlDataReader reader)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            while (reader.Read())
            {
                T obj = Activator.CreateInstance<T>();//创建指定类型实例
                foreach (System.Reflection.PropertyInfo p in fields)
                {
                    if (!Convert.IsDBNull(reader[p.Name]))
                    {
                        p.SetValue(obj, Convert.ChangeType(reader[p.Name], p.PropertyType), null);
                    }
                }
                list.Add(obj);
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 执行sql查询语句，返回List&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(string strSQL)
        {
            return ToList<T>(GetDataTable(strSQL));
        }
        /// <summary>
        /// 执行sql查询语句，返回List&lt;T&gt;（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>List集合</returns>
        public static List<T> ToList<T>(string strSQL, SqlConnection newConn)
        {
            return ToList<T>(GetDataTable(strSQL, newConn));
        }
        /// <summary>
        /// 执行sql查询语句，返回序列中的第一个元素；如果序列中不包含任何元素，则返回默认值。
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>实体对象</returns>
        public static T FirstOrDefault<T>(string strSQL)
        {
            return ToList<T>(strSQL).FirstOrDefault();
        }
        /// <summary>
        /// 执行sql查询语句，返回序列中的第一个元素；如果序列中不包含任何元素，则返回默认值。
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>实体对象</returns>
        public static T FirstOrDefault<T>(string strSQL, SqlConnection newConn)
        {
            return ToList<T>(strSQL, newConn).FirstOrDefault();
        }
        /// <summary>
        /// 添加的方法（反射）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="removeColumns">需要移除的属性列</param>
        /// <returns>受影响的行数</returns>
        public static int Insert<T>(T obj, string[] removeColumns = null)
        {
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            //SQL语句
            StringBuilder strSQL = new StringBuilder();
            //需要插入表的字段
            StringBuilder strField = new StringBuilder();
            //需要插入表的值
            StringBuilder strValue = new StringBuilder();
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (PropertyInfo p in fields)
            {
                if (removeColumns != null && removeColumns.Length != 0 && removeColumns.Contains(p.Name))
                {
                    continue;
                }
                strField.AppendFormat("{0},", p.Name);
                strValue.AppendFormat("@{0},", p.Name);
                arrParameter.Add(new SqlParameter("@" + p.Name, p.GetValue(obj, null) ?? DBNull.Value));
            }
            strSQL.AppendFormat("insert into {0}(", t.Name);
            strSQL.Append(strField.ToString().Remove(strField.Length - 1));
            strSQL.Append(") values (");
            strSQL.Append(strValue.ToString().Remove(strValue.Length - 1));
            strSQL.Append(")");
            return DBHelper.ExecuteCommand(strSQL.ToString(), arrParameter.ToArray());
        }
        /// <summary>
        /// 添加的方法（反射）（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="removeColumns">需要移除的属性列</param>
        /// <returns>受影响的行数</returns>
        public static int Insert<T>(T obj, SqlConnection newConn, string[] removeColumns = null)
        {
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            //SQL语句
            StringBuilder strSQL = new StringBuilder();
            //需要插入表的字段
            StringBuilder strField = new StringBuilder();
            //需要插入表的值
            StringBuilder strValue = new StringBuilder();
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (PropertyInfo p in fields)
            {
                if (removeColumns != null && removeColumns.Length != 0 && removeColumns.Contains(p.Name))
                {
                    continue;
                }
                strField.AppendFormat("{0},", p.Name);
                strValue.AppendFormat("@{0},", p.Name);
                arrParameter.Add(new SqlParameter("@" + p.Name, p.GetValue(obj, null) ?? DBNull.Value));
            }
            strSQL.AppendFormat("insert into {0}(", t.Name);
            strSQL.Append(strField.ToString().Remove(strField.Length - 1));
            strSQL.Append(") values (");
            strSQL.Append(strValue.ToString().Remove(strValue.Length - 1));
            strSQL.Append(")");
            return DBHelper.ExecuteCommand(strSQL.ToString(), newConn, arrParameter.ToArray());
        }
        /// <summary>
        /// 修改的方法（反射）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="primaryKey">主键列名称</param>
        /// <param name="removeColumns">需要移除的属性列</param>
        /// <returns>受影响的行数</returns>
        public static int Update<T>(T obj, string primaryKey, string[] removeColumns = null)
        {
            Type t = typeof(T);
            object pkValue = null;
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("update {0} set ", t.Name);
            PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (PropertyInfo p in fields)
            {
                var value = p.GetValue(obj, null);
                if (p.Name == primaryKey)
                {
                    pkValue = value;
                }
                else if (removeColumns != null && removeColumns.Length != 0 && removeColumns.Contains(p.Name))
                {
                    continue;
                }
                else
                {
                    strSQL.AppendFormat("{0}=@{0},", p.Name);
                    arrParameter.Add(new SqlParameter("@" + p.Name, value ?? DBNull.Value));
                }
            }
            strSQL.Remove(strSQL.ToString().LastIndexOf(","), 1);
            strSQL.AppendFormat(" where {0}=@{0}", primaryKey);
            arrParameter.Add(new SqlParameter("@" + primaryKey, pkValue));
            return DBHelper.ExecuteCommand(strSQL.ToString(), arrParameter.ToArray());
        }
        /// <summary>
        /// 修改的方法（反射）（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <param name="primaryKey">主键列名称</param>
        /// <param name="removeColumns">需要移除的属性列</param>
        /// <returns>受影响的行数</returns>
        public static int Update<T>(T obj, string primaryKey, SqlConnection newConn, string[] removeColumns = null)
        {
            Type t = typeof(T);
            object pkValue = null;
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("update {0} set ", t.Name);
            PropertyInfo[] fields = t.GetProperties();//获取指定对象的所有公共属性
            foreach (PropertyInfo p in fields)
            {
                var value = p.GetValue(obj, null);
                if (p.Name == primaryKey)
                {
                    pkValue = value;
                }
                else if (removeColumns != null && removeColumns.Length != 0 && removeColumns.Contains(p.Name))
                {
                    continue;
                }
                else
                {
                    strSQL.AppendFormat("{0}=@{0},", p.Name);
                    arrParameter.Add(new SqlParameter("@" + p.Name, value ?? DBNull.Value));
                }
            }
            strSQL.Remove(strSQL.ToString().LastIndexOf(","), 1);
            strSQL.AppendFormat(" where {0}=@{0}", primaryKey);
            arrParameter.Add(new SqlParameter("@" + primaryKey, pkValue));
            return DBHelper.ExecuteCommand(strSQL.ToString(), newConn, arrParameter.ToArray());
        }
        /// <summary>
        /// 删除的方法（反射）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="primaryKey">主键列名称</param>
        /// <returns>受影响的行数</returns>
        public static int Delete<T>(T obj, string primaryKey)
        {
            Type t = typeof(T);
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            string strSQL = string.Format("delete from {0} where {1}=@{1}", t.Name, primaryKey);
            var p = t.GetProperty(primaryKey);
            arrParameter.Add(new SqlParameter("@" + primaryKey, p.GetValue(obj, null)));
            return DBHelper.ExecuteCommand(strSQL, arrParameter.ToArray());
        }
        /// <summary>
        /// 删除的方法（反射）（指定数据库连接串，适用于跨数据库操作）
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="primaryKey">主键列名称</param>
        /// <param name="newConn">SqlConnection数据库连接串</param>
        /// <returns>受影响的行数</returns>
        public static int Delete<T>(T obj, string primaryKey, SqlConnection newConn)
        {
            Type t = typeof(T);
            List<SqlParameter> arrParameter = new List<SqlParameter>();
            string strSQL = string.Format("delete from {0} where {1}=@{1}", t.Name, primaryKey);
            var p = t.GetProperty(primaryKey);
            arrParameter.Add(new SqlParameter("@" + primaryKey, p.GetValue(obj, null)));
            return DBHelper.ExecuteCommand(strSQL, newConn, arrParameter.ToArray());
        }
        #endregion
    }
}
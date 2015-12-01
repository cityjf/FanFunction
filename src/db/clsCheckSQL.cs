using System;
using System.Collections.Generic;
using System.Text;

namespace FanFunction
{
    /// <summary>
    /// 用于检查sql语句是否包含非法字符
    /// </summary>
    public class clsCheckSQL
    {
        /// <summary>
        /// 非法关键词
        /// </summary>
        private const string keyword = "select|master|group|drop|or|delete|exec|insert|update|declare|create|where|dbo|database";


        /// <summary>
        /// 检查sql语句是否包含非法关键词（select|master|group|drop|or|delete|exec|insert|update|declare|create|where|dbo|database）
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns>不包含非法关键词则返回True，否则抛出异常</returns>
        public static bool CheckStrSQL(string strSql)
        {
            string newSqlKey = string.Empty;
            string[] arrKeyword = keyword.Split('|');//根据丨线分隔
            string[] arrSqlWhere = strSql.ToLower().Split(' ');//根据空格分隔
            //循环检查是否含有非法关键词
            foreach (string sqlKey in arrSqlWhere)
            {
                newSqlKey = sqlKey;
                //排除首词的(
                if (newSqlKey.StartsWith("("))
                {
                    newSqlKey = newSqlKey.Substring(1);
                }
                //排除末词的*
                if (newSqlKey.EndsWith("*"))
                {
                    newSqlKey = newSqlKey.Substring(0, newSqlKey.Length - 1);
                }
                foreach (string key in arrKeyword)
                {
                    if (key == newSqlKey.Trim())
                    {
                        throw new Exception("包含非法关键词[" + key + "]请检查！");
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 检查sql语句是否包含非法关键词，排除指定的关键词不检查（select|master|group|drop|or|delete|exec|insert|update|declare|create|where|dbo|database）
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="noCheckKey">排除的关键词列表</param>
        /// <returns>不包含非法关键词则返回True，否则抛出异常</returns>
        public static bool CheckStrSQL(string strSql, List<string> noCheckKey)
        {
            string newSqlKey = string.Empty;
            string newKeyword = keyword;
            //循环排除掉keyword中不检查的关键词
            foreach (string strNoCheckKey in noCheckKey)
            {
                //因为database是keyword的最后一个关键词
                if (strNoCheckKey.Trim() != "database")
                {
                    newKeyword = newKeyword.Replace(strNoCheckKey.Trim() + "|", "");
                }
                else
                {
                    newKeyword = newKeyword.Replace("|" + strNoCheckKey.Trim(), "");
                }
            }
            string[] arrKeyword = newKeyword.Split('|');//根据丨线分隔
            string[] arrSqlWhere = strSql.ToLower().Split(' ');//根据空格分隔
            //循环检查是否含有非法关键词
            foreach (string sqlKey in arrSqlWhere)
            {
                newSqlKey = sqlKey;
                //排除首词的(
                if (newSqlKey.StartsWith("("))
                {
                    newSqlKey = newSqlKey.Substring(1);
                }
                //排除末词的*
                if (newSqlKey.EndsWith("*"))
                {
                    newSqlKey = newSqlKey.Substring(0, newSqlKey.Length - 1);
                }
                foreach (string key in arrKeyword)
                {
                    if (key == newSqlKey.Trim())
                    {
                        throw new Exception("包含非法关键词[" + key + "]请检查！");
                    }
                }
            }
            return true;
        }
    }
}

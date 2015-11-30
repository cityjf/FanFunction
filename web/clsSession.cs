using System;
using System.Collections.Generic;
using System.Text;

namespace FanFunction
{
    /// <summary>
    /// web项目用于存取Session的类
    /// </summary>
    public class clsSession
    {
        /// <summary>
        /// 用户名(UserId)
        /// </summary>
        public string UserId
        {
            get
            {
                //不要用Session["UserId"].ToString()的方式，为空时会报错(string)这种不报错
                return (string)System.Web.HttpContext.Current.Session["UserId"];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add("UserId", value);
            }
        }
        /// <summary>
        /// 用户姓名(UserName)
        /// </summary>
        public static string UserName
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["UserName"];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add("UserName", value);
            }
        }
        /// <summary>
        /// 错误的信息(ErrorMsg)
        /// </summary>
        public static string ErrorMsg
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["ErrorMsg"];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add("ErrorMsg", value);
            }
        }
        /// <summary>
        /// 角色Id(RoleId)
        /// </summary>
        public static string RoleId
        {
            get
            {
                return (string)System.Web.HttpContext.Current.Session["RoleId"];
            }
            set
            {
                System.Web.HttpContext.Current.Session.Add("RoleId", value);
            }
        }
    }
}

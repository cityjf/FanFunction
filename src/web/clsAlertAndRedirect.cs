using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace FanFunction
{
    /// <summary>
    /// web网页用于弹出消息和页面跳转的类
    /// </summary>
    public class clsAlertAndRedirect
    {
        /// <summary>
        /// 跳转到错误页面，并把错误信息存入Session，存入的Session键为ErrorMsg
        /// </summary>
        /// <param name="strErrorMsg">错误提示</param>
        /// <param name="strErrorPage">要跳转的错误界面的路径</param>
        public static void RedirectErrorPage(string strErrorMsg, string strErrorPage)
        {
            HttpContext.Current.Session["ErrorMsg"] = strErrorMsg;
            HttpContext.Current.Response.Redirect(strErrorPage);
        }        
        /// <summary>
        /// 弹出提示框，可能导致界面变形【不推荐】
        /// </summary>
        /// <param name="message">消息内容</param>
        public static void Alert(string message)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            System.Web.HttpContext.Current.Response.Write(string.Format(js, message));
        }
        /// <summary>
        /// 弹出消息框，不会导致界面变形【推荐】
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="page">当前页面对象，如this.Page</param>
        public static void Alert(string message, System.Web.UI.Page page)
        {
            string js = @"<Script language='JavaScript'>alert('" + message + "');</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "alert"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "alert", js);
            }
        }
        /// <summary> 
        /// 弹出消息框并跳转向到新的URL 
        /// </summary> 
        /// <param name="message">消息内容</param> 
        /// <param name="toURL">跳转页面的地址</param> 
        /// <param name="page">当前页面对象，如this.Page</param>
        public static void AlertAndRedirect(string message, string toURL, Page page)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "AlertAndRedirect"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "AlertAndRedirect", string.Format(js, message, toURL));
            }
        }
        /// <summary>
        /// 弹出消息框并且跳转向到新的URL ,不需要页面对象
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toURL">跳转的地址</param>
        public static void AlertAndRedirect(string message, string toURL)
        {
            //此if是专门处理session失效的问题
            if (toURL.Contains("~/"))
            {
                if (toURL.Substring(0, 2) == "~/")
                {
                    //Url是页面的完整路径  CurrentExecutionFilePath是文件的路径  为了能返回到项目下的login.aspx
                    toURL = HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.CurrentExecutionFilePath, "/" + toURL.Substring(2));
                }
            }
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
        }
        /// <summary> 
        /// 返回上一页
        /// </summary> 
        /// <param name="page">当前页面对象，如this.Page</param>
        public static void GoHistory(Page page)
        {
            string js = @"<Script language='JavaScript'>history.go(-2);</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "GoHistory"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "GoHistory", js);
            }
        }
        /// <summary> 
        /// 关闭当前窗口 
        /// </summary> 
        public static void CloseWindow()
        {
            string js = @"<Script language='JavaScript'>parent.opener=null;window.close();</Script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 关闭本窗口并刷新父窗口
        /// </summary>
        /// <param name="url">父窗口地址</param>
        /// <param name="page">当前页面对象，如this.Page</param>
        public static void RefreshParent(string url, Page page)
        {
            string js = @"<Script language='JavaScript'>window.opener.location.href='" + url + "';window.close();</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "RefreshParent"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "RefreshParent", js);
            }
        }
        /// <summary> 
        /// 刷新打开窗口 
        /// </summary> 
        public static void RefreshOpener(Page page)
        {
            string js = @"<Script language='JavaScript'>opener.location.reload();</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "RefreshOpener"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "RefreshOpener", js);
            }
        }
        /// <summary> 
        /// 打开指定大小的新窗体 
        /// </summary> 
        /// <param name="url">打开页面的地址</param> 
        /// <param name="width">宽度</param> 
        /// <param name="heigth">高度</param> 
        /// <param name="top">距顶部的位置</param> 
        /// <param name="left">距左的位置</param> 
        /// <param name="page">页面对象</param> 
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left, Page page)
        {
            string js = @"<Script language='JavaScript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</Script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "OpenWebFormSize"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "OpenWebFormSize", js);
            }
        }
        /// <summary>
        /// 弹出模式对话框（居中显示）
        /// </summary>
        /// <param name="strUrl">页面</param>
        /// <param name="width">弹出页面的宽度</param>
        /// <param name="height">弹出页面的高度</param>
        /// <param name="page">页面对象</param>
        public static void ShowModalDialog(string strUrl, int width, int height, Page page)
        {
            string js = "<script language=javascript>showModalDialog('" + strUrl + "','dialogWidth=" + width + "px;dialogHeight=" + height + "px;scroll:yes;status=no;');</script>";
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "ShowModalDialog"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowModalDialog", js);
            }
        }
        /// <summary> 
        /// 打开指定大小位置的模式对话框 
        /// </summary> 
        /// <param name="webFormUrl">打开模式对话框页面的地址</param> 
        /// <param name="width">宽度</param> 
        /// <param name="height">高高</param> 
        /// <param name="top">距顶部的位置</param> 
        /// <param name="left">距左的位置</param> 
        /// <param name="page">页面对象</param> 
        public static void ShowModalDialog(string webFormUrl, int width, int height, int top, int left, Page page)
        {
            string features = "dialogWidth:" + width.ToString() + "px"
            + ";dialogHeight:" + height.ToString() + "px"
            + ";dialogLeft:" + left.ToString() + "px"
            + ";dialogTop:" + top.ToString() + "px"
            + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features, page);
        }
        /// <summary> 
        /// 弹出模态窗口 
        /// </summary> 
        /// <param name="webFormUrl"></param> 
        /// <param name="features"></param> 
        private static void ShowModalDialogWindow(string webFormUrl, string features, Page page)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "ShowModalDialogWindow"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "ShowModalDialogWindow", js);
            }
        }
        /// <summary> 
        /// 弹出模式对话框 
        /// </summary> 
        /// <param name="webFormUrl"></param> 
        /// <param name="features"></param> 
        /// <returns></returns> 
        private static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            return @"<script language=javascript>showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
        }
        /// <summary> 
        /// 跳转页面【此方法跳转的页面无法用浏览器的前进后退返回】
        /// </summary> 
        /// <param name="url">跳转的地址</param> 
        public static void RedirectReplace(string url, Page page)
        {
            string js = @"<Script language='JavaScript'>window.location.replace('{0}');</Script>";
            js = string.Format(js, url);
            if (!page.ClientScript.IsStartupScriptRegistered(page.GetType(), "JavaScriptLocationHref"))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), "JavaScriptLocationHref", js);
            }
        }
    }
}
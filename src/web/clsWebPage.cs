using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace FanFunction
{
    /// <summary>
    ///  web项目用于Asp.Net页面操作的类,其中全选CheckBox的Id必须为cbCheck
    /// </summary>
    public class clsWebPage
    {
        /// <summary>
        /// 获取GridView选中的行的第一条数据的DataKeys的值
        /// </summary>
        /// <param name="gv">GridView对象</param>
        /// <param name="strCheckBoxID">CheckBox的ID</param>
        /// <returns>GridView选中的行的第一条数据的DataKeys的值</returns>
        public static string GetFirstItemIdByGV(GridView gv, string strCheckBoxID)
        {
            string strFirstItem = string.Empty;
            if (gv.Rows.Count != 0)
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gv.Rows[i].FindControl(strCheckBoxID);
                    if ((cb != null) && cb.Checked)
                    {
                        strFirstItem = gv.DataKeys[i].Value.ToString();
                    }
                }
            }
            return strFirstItem;
        }
        /// <summary>
        /// 获取GridView选中的行的DataKeys集合的值
        /// </summary>
        /// <param name="gv">GridView对象</param>
        /// <param name="strCheckBoxID">CheckBox的ID</param>
        /// <returns>GridView选中的行的DataKeys集合的值</returns>
        public static List<string> GetCheckedItemIdByGV(GridView gv, string strCheckBoxID)
        {
            List<string> arrID = null;
            if (gv.Rows.Count != 0)
            {
                arrID = new List<string>();
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gv.Rows[i].FindControl(strCheckBoxID);
                    if ((cb != null) && cb.Checked)
                    {
                        arrID.Add(gv.DataKeys[i].Value.ToString());
                    }
                }
            }
            return arrID;
        }
        /// <summary>
        /// 获取GridView选中的行的DataKeys集合的值(当有多个DataKeyNames的时候可以动态选择指定索引)
        /// </summary>
        /// <param name="gv">GridView对象</param>
        /// <param name="strCheckBoxID">CheckBox的ID</param>
        /// <param name="index">DataKeyNames的索引</param>
        /// <returns>GridView选中的行的DataKeys集合的值</returns>
        public static List<string> GetCheckedItemIdByGV(GridView gv, string strCheckBoxID, int index)
        {
            List<string> arrID = null;
            if (gv.Rows.Count != 0)
            {
                arrID = new List<string>();
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gv.Rows[i].FindControl(strCheckBoxID);
                    if ((cb != null) && cb.Checked)
                    {
                        arrID.Add(gv.DataKeys[i][index].ToString());
                    }
                }
            }
            return arrID;
        }
        /// <summary>
        /// 选中GridView的行（选中DataKeys的值指定的GridView的行）
        /// </summary>
        /// <param name="gv">GridView对象</param>
        /// <param name="strCheckBoxID">CheckBox的ID</param>
        /// <param name="id">DataKeys的值</param>
        public static void SetCheckedItemById(GridView gv, string strCheckBoxID, string id)
        {
            if (gv.Rows.Count != 0)
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {
                    CheckBox cb = (CheckBox)gv.Rows[i].FindControl(strCheckBoxID);
                    if ((cb != null) && gv.DataKeys[i].Value.ToString() == id)
                    {
                        cb.Checked = true;
                    }
                }
            }
        }
        /// <summary>
        /// 全选/全不选 效果
        /// </summary>
        /// <param name="gv">GridView对象</param>
        /// <param name="strCheckBoxID">CheckBox的ID</param>
        /// <param name="IsChecked">是全选还是全不选</param>
        public static void GridViewChecked(GridView gv, string strCheckBoxID, bool IsChecked)
        {
            for (int i = 0; i <= gv.Rows.Count - 1; i++)
            {
                CheckBox cbox = (CheckBox)gv.Rows[i].FindControl(strCheckBoxID);
                if (IsChecked)
                {
                    cbox.Checked = true;
                }
                else
                {
                    cbox.Checked = false;
                }
            }

        }
        /// <summary>
        /// 为Controls集合里所有文本框添加onfocus，onblur，onmousemove，onmouseout属性
        /// </summary>
        /// <param name="controls">容器，例：this.form1.Controls</param>
        public static void SetTextBox(ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is TextBox)
                {
                    TextBox tx = c as TextBox;
                    tx.Attributes["class"] = "input_out";
                    tx.Attributes["onfocus"] = "className='input_on';";
                    tx.Attributes["onblur"] = "className='input_off';";
                    tx.Attributes["onmousemove"] = "className='input_move'";
                    tx.Attributes["onmouseout"] = "className='input_out'";
                }
                //递归
                else if (c.HasControls())
                {
                    SetTextBox(c.Controls);
                }
            }
        }
        /// <summary>
        /// 为Controls集合里除arrNoId集合外的文本框添加onfocus，onblur，onmousemove，onmouseout属性
        /// </summary>
        /// <param name="controls">容器，例：this.form1.Controls</param>
        /// <param name="arrNo">不需要添加属性的文本框Id集合</param>
        public static void SetTextBox(ControlCollection controls, List<string> arrNoId)
        {
            foreach (Control c in controls)
            {
                if (c is TextBox && !arrNoId.Contains(c.ID))
                {
                    TextBox tx = c as TextBox;
                    tx.Attributes["class"] = "input_out";
                    tx.Attributes["onfocus"] = "className='input_on';";
                    tx.Attributes["onblur"] = "className='input_off';";
                    tx.Attributes["onmousemove"] = "className='input_move'";
                    tx.Attributes["onmouseout"] = "className='input_out'";
                }
                //递归
                else if (c.HasControls())
                {
                    SetTextBox(c.Controls);
                }
            }
        }
        /// <summary>
        /// 是否有非法字符，验证包括sql关键词和html关键词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckStrSQL(string str)
        {
            str = str.ToLower();//先转换成小写形式
            bool bo = true;
            if (str.Contains("select") || str.Contains("where") || str.Contains(";") || str.Contains("drop") || str.Contains("delete") || str.Contains("<") || str.Contains(">") || str.Contains("=") || str.Contains("&"))
            {
                bo = true;
            }
            else if (Regex.IsMatch(str, @"<frameset[\s\S]+</frameset *>") || Regex.IsMatch(str, @"<iframe[\s\S]+</iframe *>") || Regex.IsMatch(str, @" on[\s\S]*=") || Regex.IsMatch(str, @" href *= *[\s\S]*script *:") || Regex.IsMatch(str, @"<script[\s\S]+</script *>"))
            {
                bo = true;
            }
            else if (str.Contains("<applet>") || str.Contains("<body>") || str.Contains("<embed>") || str.Contains("<frame>") || str.Contains("<script>") || str.Contains("<frameset>") || str.Contains("<html>") || str.Contains("<iframe>") || str.Contains("<img>") || str.Contains("<style>") || str.Contains("<layer>") || str.Contains("<link>") || str.Contains("<ilayer>") || str.Contains("<meta>") || str.Contains("<object>"))
            {
                bo = true;
            }
            else
            {
                bo = false;
            }
            return bo;
        }
        /// <summary>
        /// 清除html标记
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string NoHTML(string Htmlstring)
        {
            return System.Text.RegularExpressions.Regex.Replace(Htmlstring, "<[^>]*>", "").Trim();
        }
    }
}